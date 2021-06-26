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

        public Dictionary<int, StrategemProperties> idToStrategem = new Dictionary<int, StrategemProperties>();

        public override void Start()
        {
            base.Start();
            foreach (var a in strategemProperties)
            {
                idToStrategem.Add(a.strategemID, a);
            }
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName == GameplayScene)
            {
                dedicatedServer = Instantiate(dedicatedServerPrefab);
                NetworkServer.Spawn(dedicatedServer);
            }
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            gamePlayer.GetComponent<NetworkPlayerScript>().characterData.swapTeam(roomPlayer.GetComponent<NetworkRoomPlayerScript>().team);

            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);

        }
    }
}
