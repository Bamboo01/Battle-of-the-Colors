using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Bamboo.Events;
using CSZZGame.Networking;
using CSZZGame.Refactor;
using Bamboo.UI;
using DG.Tweening;


public class GameController : NetworkBehaviour
{
    [SerializeField] double MatchDuration = 300;
    NetworkRoomManagerScript networkManager;
    double initialGameTime = 0;
    double currentGameTime = 0;
    bool gameEnded;
    float StartTime = 2.0f;

    //I AM CODING THIS AT 4AM
    Queue<GameObject> team1LiveContainer = new Queue<GameObject>();
    Queue<GameObject> team2LiveContainer = new Queue<GameObject>(); 
    Queue<GameObject> team1DeadContainer = new Queue<GameObject>();
    Queue<GameObject> team2DeadContainer = new Queue<GameObject>();

    [SerializeField] TextMeshProUGUI timerText;

    [Header("Prefabs")]
    [SerializeField] string bgmName;
    [SerializeField] string hypebgmName;

    [Header("Prefabs")]
    [SerializeField] GameObject Team1LivePrefabUI;
    [SerializeField] GameObject Team1DeadPrefabUI;
    [SerializeField] GameObject Team2LivePrefabUI;
    [SerializeField] GameObject Team2DeadPrefabUI;

    [Header("UI")]
    [SerializeField] GameObject team1PlayersContainer;
    [SerializeField] GameObject team2PlayersContainer;
    [SerializeField] Image healthFill;
    [SerializeField] Image team1Fill;
    [SerializeField] TMP_Text team1Text;
    [SerializeField] Image team2Fill;
    [SerializeField] TMP_Text team2Text;
    [SerializeField] Transform endTarget;
    [SerializeField] RectTransform readyUIGameObject;
    [SerializeField] Image hitmarkerUIGameObject;
    [SerializeField] RectTransform goUIGameObject;
    [SerializeField] RectTransform waitingUIGameObject;
    [SerializeField] RectTransform lastMinUIGameObject;

    [SyncVar] int maxHealth = 1;

    public Dictionary<ServerCharacterData.CHARACTER_TEAM, List<Transform>> teamToStartPositionList = new Dictionary<ServerCharacterData.CHARACTER_TEAM, List<Transform>>();
    public Dictionary<int, StrategemProperties> idToStrategem = new Dictionary<int, StrategemProperties>();

    bool isHypeCalled = false;
    bool isGameStarted = false;
    int numLoaded = 0;

    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeServer();
        
        EventManager.Instance.Listen(EventChannels.OnServerClientLoadedIntoGame, OnServerClientLoadedIntoGame);
        EventManager.Instance.Listen(EventChannels.OnServerAllClientsLoadedIntoGame, OnServerAllClientsLoadedIntoGame);
        EventManager.Instance.Listen(EventChannels.OnClientGameEnd, OnClientGameEnd);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        InitializeServer();
        gameEnded = false;

        EventManager.Instance.Listen(EventChannels.OnTargetClientPlayerDeath, OnTargetClientPlayerDeath);
        EventManager.Instance.Listen(EventChannels.OnClientPlayerDeath, OnClientPlayerDeath);
        EventManager.Instance.Listen(EventChannels.OnTargetClientPlayerSpawn, OnTargetClientPlayerSpawn);
        EventManager.Instance.Listen(EventChannels.OnClientPlayerSpawn, OnClientPlayerSpawn);
        EventManager.Instance.Listen(EventChannels.OnHealthChange, OnHealthChange);
        EventManager.Instance.Listen(EventChannels.OnClientLoadedIntoGame, OnClientLoadedIntoGame);
        EventManager.Instance.Listen(EventChannels.OnAllClientsLoadedIntoGame, OnAllClientsLoadedIntoGame);
        EventManager.Instance.Listen(EventChannels.OnClientGameReachedHype, OnClientGameReachedHype);
        EventManager.Instance.Listen(EventChannels.OnClientGameEnd, OnClientGameEnd);
        EventManager.Instance.Listen(EventChannels.OnTargetClientHitSomething, OnTargetClientHitSomething);
    }

    void InitializeServer()
    {
        networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
        initialGameTime = NetworkTime.time;
        gameEnded = false;
        maxHealth = networkManager.maxPlayerHealth;
    }

    void Awake()
    {
        teamToStartPositionList = new Dictionary<ServerCharacterData.CHARACTER_TEAM, List<Transform>>();
        teamToStartPositionList.Add(ServerCharacterData.CHARACTER_TEAM.TEAM_1, SpawnPointManager.Instance.team1SpawnPoints);
        teamToStartPositionList.Add(ServerCharacterData.CHARACTER_TEAM.TEAM_2, SpawnPointManager.Instance.team2SpawnPoints);
    }

    private void Update()
    {
        if (isGameStarted)
        {
            if (NetworkTime.time < initialGameTime + MatchDuration)
            {
                currentGameTime = MatchDuration - (NetworkTime.time - initialGameTime);
                TimeSpan t = TimeSpan.FromSeconds(currentGameTime);
                string timeString = "";
                if (t.Minutes > 0)
                {
                    timeString = t.ToString(@"mm\:ss");
                }
                else
                {
                    timeString = t.ToString(@"ss\:ff");
                }
                timerText.text = timeString;

                // TODO: This could honestly be a coroutine, but I'm really lazy, let's do it next time!
                if (isServer && !isHypeCalled && currentGameTime < 60.0f)
                {
                    isHypeCalled = true;
                    CSZZNetworkInterface.Instance.SendNetworkEvent(EventChannels.OnClientGameReachedHype);
                }
            }
            else if (!gameEnded && isServer)
            {
                // I wan die alr
                gameEnded = true;
                NetworkPainterManager.Instance.paintCalculator.StartCalculatingPaint();
                var scores = NetworkPainterManager.Instance.paintCalculator.colorCounterList;
                CSZZNetworkInterface.Instance.SendNetworkEvent(EventChannels.OnClientGameEnd, new SerializablePaintScore(scores[0], scores[1]));
            }
        }
    }

    void OnTargetClientHitSomething(IEventRequestInfo info)
    {
        SoundManager.Instance.PlaySoundByName("Damage");
        hitmarkerUIGameObject.gameObject.SetActive(true);
        hitmarkerUIGameObject.DOFade(1.0f, 0.1f).OnComplete(() =>
        {
            hitmarkerUIGameObject.DOFade(1.0f, 0.0f);
            hitmarkerUIGameObject.DOFade(0.0f, 0.4f).OnComplete(() =>
            {
                hitmarkerUIGameObject.gameObject.SetActive(false);
            }).Play();
        }).Play();
    }

    void OnServerAllClientsLoadedIntoGame(IEventRequestInfo info)
    {
        // Begin a countdown and send events to client to start counting down
        CSZZNetworkInterface.Instance.SendNetworkEvent(EventChannels.OnAllClientsLoadedIntoGame);
        StartCoroutine(DelayedStartGame(StartTime));
        isGameStarted = true;
        initialGameTime = NetworkTime.time;
    }

    void OnClientGameReachedHype(IEventRequestInfo info)
    {
        SoundManager.Instance.StopSoundByName(bgmName);
        SoundManager.Instance.PlaySoundByName(hypebgmName);
        lastMinUIGameObject.gameObject.SetActive(true);
        lastMinUIGameObject.DORotate(new Vector3(0, 0, -340), 1.5f, RotateMode.LocalAxisAdd).OnComplete(
            () =>
            {
                lastMinUIGameObject.DOScale(Vector3.zero, 0.5f).OnComplete(
                    () => 
                    { 
                        lastMinUIGameObject.gameObject.SetActive(false); 
                    }
                    ).Play();
            }
            ).Play();
    }

    void OnAllClientsLoadedIntoGame(IEventRequestInfo info)
    {
        waitingUIGameObject.gameObject.SetActive(false);
        // Do tweening
        MenuManager.Instance.OnlyOpenThisMenu("PregameMenu");
        readyUIGameObject.gameObject.SetActive(true);
        readyUIGameObject.DORotate(new Vector3(0, 0, -340), 1.5f, RotateMode.LocalAxisAdd).OnComplete(
            () =>
            {
                readyUIGameObject.DOScale(Vector3.zero, 0.5f).OnComplete(() => { readyUIGameObject.gameObject.SetActive(false); }).Play();
                goUIGameObject.gameObject.SetActive(true);
                goUIGameObject.DOScale(new Vector3(3.0f, 3.0f, 3.0f), 0.6f).OnPlay(
                    ()=> 
                    {
                        goUIGameObject.DORotate(new Vector3(0, 0, -340), 0.5f, RotateMode.LocalAxisAdd);
                        SoundManager.Instance.PlaySoundByName("Whistle");
                    }
                    ).OnComplete(() => 
                        {
                            goUIGameObject.DOScale(Vector3.zero, 0.2f).OnComplete(() => { goUIGameObject.gameObject.SetActive(false); }).Play().OnComplete(() =>
                            {
                                MenuManager.Instance.OnlyOpenThisMenu("GameInfoMenu");
                                MenuManager.Instance.OpenMenu("PlayerInfoMenu");
                                MenuManager.Instance.OpenMenu("StrategemMenu");
                                isGameStarted = true;
                                initialGameTime = NetworkTime.time;
                                SoundManager.Instance.PlaySoundByName(bgmName);
                            }
                            );
                        }
                        ).Play();
            }
            ).Play();
    }

    public void OnClientLoadedIntoGame(IEventRequestInfo info)
    {
        EventRequestInfo<SpawnInfo> eventRequestInfo = info as EventRequestInfo<SpawnInfo>;
        List<Transform> spawnPoints = teamToStartPositionList[eventRequestInfo.body.Team];
        CameraManager.Instance.AssignTargets(spawnPoints[eventRequestInfo.body.SpawnIndex], spawnPoints[eventRequestInfo.body.SpawnIndex]);
    }

    public void OnServerClientLoadedIntoGame(IEventRequestInfo info)
    {
        numLoaded++;
        if (numLoaded == networkManager.numPlayers)
        {
            Debug.Log("All clients have connected!");
            EventManager.Instance.Publish(EventChannels.OnServerAllClientsLoadedIntoGame, this);
        }
    }

    public void OnServerClientDisconnect(IEventRequestInfo info)
    {
        numLoaded--;
    }

    public void OnHealthChange(IEventRequestInfo info)
    {
        EventRequestInfo<int> eventRequestInfo = (EventRequestInfo<int>)(info);
        healthFill.fillAmount = ((float)eventRequestInfo.body / (float)maxHealth);
    }

    public void OnClientGameEnd(IEventRequestInfo info)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EventRequestInfo<byte[]> eventRequestInfo = (EventRequestInfo<byte[]>)(info);
        SerializablePaintScore paintScore = CSZZNetworkInterface.DeserializeEventData<SerializablePaintScore>(eventRequestInfo.body);
        int total = paintScore.team1Score + paintScore.team2Score;
        if (total == 0)
        {
            team1Fill.fillAmount = 1.0f;
            team2Fill.fillAmount = 1.0f;
            team1Text.text = 0.ToString();
            team2Text.text = 0.ToString();
        }
        else
        {
            if (paintScore.team1Score == 0)
            {
                team1Fill.fillAmount = 0.0f;
                team1Text.text = paintScore.team1Score.ToString();
            }
            else 
            {
                team1Fill.fillAmount = (float)paintScore.team1Score / (float)total;
                team1Text.text = paintScore.team1Score.ToString();
            }

            if (paintScore.team1Score == 0)
            {
                team2Fill.fillAmount = 0.0f;
                team2Text.text = paintScore.team2Score.ToString();
            }
            else
            {
                team2Fill.fillAmount = (float)paintScore.team2Score / (float)total;
                team2Text.text = paintScore.team2Score.ToString();
            }
        }
        CameraManager.Instance.AssignTargets(endTarget, endTarget);
        MenuManager.Instance.OnlyOpenThisMenu("EndGameMenu");
        SoundManager.Instance.StopAllSounds();
        SoundManager.Instance.PlaySoundByName("Whistle");
    }

    public void OnTargetClientPlayerDeath(IEventRequestInfo info)
    {
        MenuManager.Instance.OnlyOpenThisMenu("DieMenu");
    }

    public void OnTargetClientPlayerSpawn(IEventRequestInfo info)
    {
        MenuManager.Instance.OnlyOpenThisMenu("GameInfoMenu");
        MenuManager.Instance.OpenMenu("PlayerInfoMenu");
        MenuManager.Instance.OpenMenu("StrategemMenu");
    }

    public void OnClientPlayerSpawn(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<ServerCharacterData.CHARACTER_TEAM> info =  (EventRequestInfo<ServerCharacterData.CHARACTER_TEAM>)eventRequestInfo;
        if (info.body == ServerCharacterData.CHARACTER_TEAM.TEAM_1)
        {
            if (team1DeadContainer.Count > 0)
            {
                Destroy(team1DeadContainer.Dequeue());
            }
            GameObject a = Instantiate(Team1LivePrefabUI, team1PlayersContainer.transform);
            team1LiveContainer.Enqueue(a);
        }
        if (info.body == ServerCharacterData.CHARACTER_TEAM.TEAM_2)
        {
            if (team2DeadContainer.Count > 0)
            {
                Destroy(team2DeadContainer.Dequeue());
            }
            GameObject a = Instantiate(Team2LivePrefabUI, team2PlayersContainer.transform);
            team2LiveContainer.Enqueue(a);
        }
    }

    public void OnClientPlayerDeath(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<ServerCharacterData.CHARACTER_TEAM> info = (EventRequestInfo<ServerCharacterData.CHARACTER_TEAM>)eventRequestInfo;
        if (info.body == ServerCharacterData.CHARACTER_TEAM.TEAM_1)
        {
            if (team1LiveContainer.Count > 0)
            {
                Destroy(team1LiveContainer.Dequeue());
            }
            GameObject a = Instantiate(Team1DeadPrefabUI, team1PlayersContainer.transform);
            team1DeadContainer.Enqueue(a);
        }
        if (info.body == ServerCharacterData.CHARACTER_TEAM.TEAM_2)
        {
            if (team2LiveContainer.Count > 0)
            {
                Destroy(team2LiveContainer.Dequeue());
            }
            GameObject a = Instantiate(Team2DeadPrefabUI, team2PlayersContainer.transform);
            team2DeadContainer.Enqueue(a);
        }
    }

    public void OnLeaveGame()
    {
        if (isServer)
        {
            networkManager.StopHost();
        }
        else 
        {
            networkManager.StopClient();
        }
        networkManager.ServerChangeScene(networkManager.offlineScene);
    }

    public IEnumerator DelayedStartGame(float t)
    {
        yield return new WaitForSeconds(t);
        CSZZNetworkInterface.Instance.SendNetworkEvent(EventChannels.OnServerGameStarted);
        yield break;
    }

    #region Countdown

    #endregion
}