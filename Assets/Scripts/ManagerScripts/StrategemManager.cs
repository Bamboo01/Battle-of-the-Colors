using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Events;
using Bamboo.Utility;
using Bamboo.UI;
using CSZZGame.Networking;
using Mirror;

namespace CSZZGame.Refactor
{
    public class StrategemManager : Singleton<StrategemManager>
    {
        NetworkRoomManagerScript networkManager;
        NetworkCharacter referencedCharacter;

        [Header("Containers")]
        [SerializeField] GameObject verticalStrategemContainer;
        [SerializeField] GameObject horiztontalStrategemContainer;
        [Header("Prefabs")]
        [SerializeField] GameObject strategemUIPrefab;
        [SerializeField] GameObject arrowPrefab;

        Dictionary<int, Queue<int>> idToQueue = new Dictionary<int, Queue<int>>();
        Dictionary<int, bool> idToReady = new Dictionary<int, bool>();
        Dictionary<int, StrategemSkillContainer> idToStrategemUI = new Dictionary<int, StrategemSkillContainer>();

        bool isCallActive = false;

        protected override void OnAwake()
        {
            _persistent = false;
            base.OnAwake();
        }

        public void SetupUIManager(NetworkCharacter refchar, SyncDictionary<int, float> strategemsDictionary)
        {
            networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
            referencedCharacter = refchar;

            foreach (var a in strategemsDictionary)
            {
                GameObject go = Instantiate(strategemUIPrefab, verticalStrategemContainer.transform);
                StrategemSkillContainer container = go.GetComponent<StrategemSkillContainer>();
                container.setupContainer(networkManager.idToStrategem[a.Key].strategemSprite, networkManager.idToStrategem[a.Key].executionLength);
                idToStrategemUI.Add(a.Key, container);
                idToQueue.Add(a.Key, new Queue<int>());
                idToReady.Add(a.Key, false);
            }

            strategemDeactivation();
        }

        public void strategemActivation()
        {
            isCallActive = true;
            foreach (var a in idToStrategemUI)
            {
                a.Value.transform.SetParent(verticalStrategemContainer.transform);
                a.Value.showArrows(true);
            }
        }

        public void strategemDeactivation()
        {
            isCallActive = false;
            foreach (var a in idToStrategemUI)
            {
                a.Value.transform.SetParent(horiztontalStrategemContainer.transform);
                a.Value.showArrows(false);
            }
        }

        public int getCallableStrategem()
        {
            if (!isCallActive)
            {
                return -1;
            }
            foreach (var a in idToQueue)
            {
                if (a.Value.Count == 0 && idToReady[a.Key])
                {
                    return a.Key;
                }    
            }
            return -1;
        }



        // Callbacks
        public void OnStrategemUpdated(SyncDictionary<int, float>.Operation op, int key, float timer)
        {
            switch (op)
            {
                case SyncDictionary<int, float>.Operation.OP_ADD:
                    // THIS CALLS WHEN A STRATEGEM IS ADDED IN THE -MIDDLE- OF A GAME. BY RIGHT THIS SHOULDN'T HAPPEN UNLESS
                    // YOU HAVE SOMETHING LIKE TITANFALL WHERE YOUC AN CHOOSE YOUR SKILLS IN THE MIDDLE OF A GAME
                    Instantiate(strategemUIPrefab, horiztontalStrategemContainer.transform);
                    idToStrategemUI.Add(key, strategemUIPrefab.GetComponent<StrategemSkillContainer>());
                    idToStrategemUI[key].setupContainer(networkManager.idToStrategem[key].strategemSprite, networkManager.idToStrategem[key].executionLength);
                    break;
                case SyncDictionary<int, float>.Operation.OP_SET:
                    idToStrategemUI[key].updateFill(timer, networkManager.idToStrategem[key].cooldownTime);
                    break;
            }
        }

        public void OnStrategemReady(SyncDictionary<int, bool>.Operation op, int key, bool ready)
        {
            switch (op)
            {
                case SyncDictionary<int, bool>.Operation.OP_SET:
                    if (ready)
                    {
                        var prop = networkManager.idToStrategem[key];
                        var queue = idToQueue[key];
                        for (int i = 0; i < prop.executionLength; i++)
                        {
                            GameObject go = Instantiate(arrowPrefab, idToStrategemUI[key].arrowContainer.transform);
                            StrategemArrow arrow = go.GetComponent<StrategemArrow>();
                            int randomDir = Random.Range(0, 3);
                            arrow.SetupArrow(randomDir);
                            queue.Enqueue(randomDir);
                        }
                        idToReady[key] = true;
                    }
                    else
                    {
                        var queue = idToQueue[key];
                        queue.Clear();
                        idToStrategemUI[key].ClearArrows();
                        idToReady[key] = false;
                    }
                    break;

            }
        }

        void OnInputEvent(IEventRequestInfo eventRequestInfo)
        {
            if (!isCallActive)
            {
                return;
            }

            EventRequestInfo<InputCommand> info = (EventRequestInfo<InputCommand>)eventRequestInfo;
            int num = ConvertCommandToInt(info.body);
            if (num < 0)
            {
                return;
            }
            bool fail = true;
            foreach(var a in idToQueue)
            {
                if (a.Value.Count == 0)
                {
                    continue;
                }
                if (a.Value.Peek() == num)
                {
                    fail = false;
                    idToStrategemUI[a.Key].SetArrowFinish();
                    a.Value.Dequeue();
                }
            }

            if (fail)
            {
                ResetAllArrows();
            }
        }

        int ConvertCommandToInt(InputCommand key)
        {
            switch (key)
            {
                case InputCommand.TAP_FORWARD:
                    return 0;
                case InputCommand.TAP_BACKWARDS:
                    return 1;
                case InputCommand.TAP_LEFT:
                    return 2;
                case InputCommand.TAP_RIGHT:
                    return 3;
            }
            return -1;
        }

        void ResetAllArrows()
        {
            foreach (var a in idToStrategemUI)
            {
                if (!idToReady[a.Key])
                {
                    continue;
                }
                var prop = networkManager.idToStrategem[a.Key];
                var queue = idToQueue[a.Key];
                a.Value.ClearArrows();
                queue.Clear();
                for (int i = 0; i < prop.executionLength; i++)
                {
                    GameObject go = Instantiate(arrowPrefab, idToStrategemUI[a.Key].arrowContainer.transform);
                    StrategemArrow arrow = go.GetComponent<StrategemArrow>();
                    int randomDir = Random.Range(0, 3);
                    arrow.SetupArrow(randomDir);
                    queue.Enqueue(randomDir);
                }
            }
        }

        void Awake()
        {
            referencedCharacter = null;
        }

        void Start()
        {
            EventManager.Instance.Listen(EventChannels.OnInputEvent, OnInputEvent);
        }

        // Update is called once per frame
        void Update()
        {
            if (!referencedCharacter)
            {
                return;
            }
        }
    }
}
