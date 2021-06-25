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
        [SerializeField] ServerEventProperties serverEventProperties;
        public GameObject playerCharacterPrefab;
        public GameObject bulletPrefab;
        public GameObject dedicatedServerPrefab;
        public GameObject shieldPrefab;
        public GameObject smokePrefab;
        public GameObject markerPrefab;

        private GameObject dedicatedServer;

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName == GameplayScene)
            {
                dedicatedServer = Instantiate(dedicatedServerPrefab);
                NetworkServer.Spawn(dedicatedServer);
            }
        }
    }
}
