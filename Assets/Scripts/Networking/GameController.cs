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

public class GameController : NetworkBehaviour
{
    [SerializeField] double MatchDuration = 300;
    NetworkRoomManagerScript networkManager;
    double initialGameTime = 0;
    double currentGameTime = 0;
    bool gameEnded;

    //I AM CODING THIS AT 4AM
    Queue<GameObject> team1LiveContainer = new Queue<GameObject>();
    Queue<GameObject> team2LiveContainer = new Queue<GameObject>(); 
    Queue<GameObject> team1DeadContainer = new Queue<GameObject>();
    Queue<GameObject> team2DeadContainer = new Queue<GameObject>();

    [SerializeField] TextMeshProUGUI timerText;

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

    [SyncVar] int maxHealth = 1;

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
        EventManager.Instance.Listen(EventChannels.OnServerGameEnd, OnServerGameEnd);
    }

    void InitializeServer()
    {
        networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
        initialGameTime = NetworkTime.time;
        gameEnded = false;
        maxHealth = networkManager.maxPlayerHealth;
    }

    private void Update()
    {
        if(NetworkTime.time < initialGameTime + MatchDuration)
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
        }
        else if (!gameEnded && isServer)
        {
            // I wan die alr
            gameEnded = true;
            NetworkPainterManager.Instance.paintCalculator.StartCalculatingPaint();
            var scores = NetworkPainterManager.Instance.paintCalculator.colorCounterList;
            CSZZNetworkInterface.Instance.SendNetworkEvent(EventChannels.OnServerGameEnd, new SerializablePaintScore(scores[0], scores[1]));
        }
    }

    public void OnHealthChange(IEventRequestInfo info)
    {
        EventRequestInfo<int> eventRequestInfo = (EventRequestInfo<int>)(info);
        healthFill.fillAmount = ((float)eventRequestInfo.body / (float)maxHealth);
    }

    public void OnServerGameEnd(IEventRequestInfo info)
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
        SceneManager.LoadScene("PlaceholderMainMenu");
    }
}
