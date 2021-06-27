using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.UI;
using TMPro;
using UnityEngine.UI;

using CSZZGame.Networking;
using CSZZGame.Character;
// CSZZ - 褰╄壊鎴樹簤
namespace CSZZGame.Networking
{
    [AddComponentMenu("")]
    public class NetworkRoomManagerScript : NetworkRoomManager
    {
        [Header("Strategems (Add prefabs to network manager!)")]
        public List<StrategemProperties> strategemProperties;
        [Space(10)]
        [Header("Server-only event channels")]
        [SerializeField] ServerEventProperties serverEventProperties;
        [Space(10)]
        public GameObject playerCharacterPrefab;
        public GameObject bulletPrefab;
        public GameObject dedicatedServerPrefab;
        public GameObject shieldPrefab;
        public GameObject smokeMissilePrefab;
        public GameObject paintMissilePrefab;
        public GameObject markerPrefab;

        private GameObject dedicatedServer;

        [SerializeField] bool isDedicatedServer;

        public Dictionary<ServerCharacterData.CHARACTER_TEAM, List<Transform>> teamToStartPositionList;
        public Dictionary<int, StrategemProperties> idToStrategem = new Dictionary<int, StrategemProperties>();


        [Header("RoomUI")]
        [Space(10)]
        public TMP_InputField ipInputField;
        public Transform team1Container;
        public Transform team2Container;
        public Button ReadyButton;
        public Button SwitchTeamButton;
        public Button StartGameButton;
        public GameObject canvas;
        private NetworkIdentity localRoomPlayer;

        [Header("GameSettings")]
        public int maxPlayerHealth = 5;

        public override void Start()
        {
            isDedicatedServer = true;
            base.Start();
            foreach (var a in strategemProperties)
            {
                idToStrategem.Add(a.strategemID, a);
            }
            NetworkManagerMenuManager.Instance.OnlyOpenThisMenu("LobbyMenu");
        }

        public override void OnStartHost()
        {
            isDedicatedServer = false;
            base.OnStartHost();
        }

        public override void OnStartClient()
        {
            isDedicatedServer = false;
            base.OnStartClient();
        }

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            if (sceneName == GameplayScene)
            {
                teamToStartPositionList = new Dictionary<ServerCharacterData.CHARACTER_TEAM, List<Transform>>();
                teamToStartPositionList.Add(ServerCharacterData.CHARACTER_TEAM.TEAM_1, SpawnPointManager.Instance.team1SpawnPoints);
                teamToStartPositionList.Add(ServerCharacterData.CHARACTER_TEAM.TEAM_2, SpawnPointManager.Instance.team2SpawnPoints);
                if (isDedicatedServer)
                {
                    dedicatedServer = Instantiate(dedicatedServerPrefab);
                    NetworkServer.Spawn(dedicatedServer);
                }
            }
            base.OnRoomServerSceneChanged(sceneName);
        }

        public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
        {
            GameObject newRoomGameObject = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
            newRoomGameObject.GetComponent<NetworkRoomPlayerScript>().idTag = this.clientIndex;
            return newRoomGameObject;
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            var networkPlayerScript = gamePlayer.GetComponent<NetworkPlayerScript>();
            var networkRoomPlayerScript = roomPlayer.GetComponent<NetworkRoomPlayerScript>();
            networkPlayerScript.characterData.swapTeam(networkRoomPlayerScript.team);

            NetworkManagerMenuManager.Instance.CloseAllMenus();
            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }
        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            base.OnClientSceneChanged(conn);
            //NetworkManagerMenuManager.Instance.CloseAllMenus();
        }

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            if(newSceneName == this.GameplayScene)
            {
                NetworkManagerMenuManager.Instance.CloseAllMenus();
            }
        }
        public void OnHostGameButtonClick()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    this.StartHost();
                    NetworkManagerMenuManager.Instance.OnlyOpenThisMenu("RoomMenu");
                    StartGameButton.gameObject.SetActive(true);
                }
            }
        }

        public void OnJoinGameButtonClick()
        {
            this.networkAddress = ipInputField.text.Length > 0 ? ipInputField.text : "localhost";
            this.StartClient();
            StartGameButton.gameObject.SetActive(false);
            NetworkManagerMenuManager.Instance.OnlyOpenThisMenu("RoomMenu");
        }

        public void OnQuitGameButtonClick()
        {
            Application.Quit();
        }

        public void OnExitRoomButtonClick()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                this.StopHost();
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                this.StopClient();
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                this.StopServer();
            }
        }

        public void OnStartGameButtonClick()
        {
            //lol check doesn't work
            //if (allPlayersReady)
            //{
                ServerChangeScene(GameplayScene);
                NetworkManagerMenuManager.Instance.CloseAllMenus();
            //}
        }
    }
}
