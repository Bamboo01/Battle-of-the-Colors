using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.UI;
using TMPro;
using UnityEngine.UI;

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

        public Dictionary<int, StrategemProperties> idToStrategem = new Dictionary<int, StrategemProperties>();


        [Header("RoomUI")]
        [Space(10)]
        public TMP_InputField ipInputField;
        public Transform team1Container;
        public Transform team2Container;
        public Button ReadyButton;
        public Button SwitchTeamButton;
        public Button StartGameButton;

        private NetworkIdentity localRoomPlayer;


        public override void Start()
        {
            isDedicatedServer = true;
            base.Start();
            foreach (var a in strategemProperties)
            {
                idToStrategem.Add(a.strategemID, a);
            }
            MenuManager.Instance.OnlyOpenThisMenu("LobbyMenu");
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
            if (sceneName == GameplayScene && isDedicatedServer)
            {
                dedicatedServer = Instantiate(dedicatedServerPrefab);
                NetworkServer.Spawn(dedicatedServer);
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

            MenuManager.Instance.CloseAllMenus();
            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }
        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            base.OnClientSceneChanged(conn);
            //MenuManager.Instance.CloseAllMenus();
        }

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            if(newSceneName == this.GameplayScene)
            {
                MenuManager.Instance.CloseAllMenus();
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
                    MenuManager.Instance.OnlyOpenThisMenu("RoomMenu");
                    StartGameButton.gameObject.SetActive(true);
                }
            }
        }

        public void OnJoinGameButtonClick()
        {
            this.networkAddress = ipInputField.text.Length > 0 ? ipInputField.text : "localhost";
            this.StartClient();
            StartGameButton.gameObject.SetActive(false);
            MenuManager.Instance.OnlyOpenThisMenu("RoomMenu");
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
                MenuManager.Instance.CloseAllMenus();
            //}
        }
    }
}
