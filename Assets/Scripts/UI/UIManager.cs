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
    public class UIManager : Singleton<UIManager>
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
        Dictionary<int, StrategemSkillContainer> idToStrategemUI = new Dictionary<int, StrategemSkillContainer>();

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
                container.setupContainer(networkManager.idToStrategem[a.Key].strategemSprite);
                idToStrategemUI.Add(a.Key, container);
            }
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
                    idToStrategemUI[key].setupContainer(networkManager.idToStrategem[key].strategemSprite);
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
                    var prop = networkManager.idToStrategem[key];
                    var queue = new Queue<int>();
                    for (int i = 0; i < prop.executionLength; i++)
                    {
                        GameObject go = Instantiate(arrowPrefab, idToStrategemUI[key].arrowContainer.transform);
                        StrategemArrow arrow = go.GetComponent<StrategemArrow>();
                        int randomDir = Random.Range(0, 3);
                        arrow.SetupArrow(randomDir);
                        queue.Enqueue(randomDir);
                    }
                    idToQueue.Add(key, queue);
                    break;
            }
        }

        void Awake()
        {
            referencedCharacter = null;
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
