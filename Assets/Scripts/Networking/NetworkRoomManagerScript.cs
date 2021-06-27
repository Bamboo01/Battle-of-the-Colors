using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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

        public override void Start()
        {
            isDedicatedServer = true;
            base.Start();
            foreach (var a in strategemProperties)
            {
                idToStrategem.Add(a.strategemID, a);
            }
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

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            var networkPlayerScript = gamePlayer.GetComponent<NetworkPlayerScript>();
            networkPlayerScript.characterData.swapTeam(roomPlayer.GetComponent<NetworkRoomPlayerScript>().team);
            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }
    }
}
