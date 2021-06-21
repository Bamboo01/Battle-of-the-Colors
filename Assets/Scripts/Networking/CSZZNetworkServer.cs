using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Utility;

namespace CSZZGame.Networking
{
    public class CSZZNetworkServer : Singleton<CSZZNetworkServer>
    {
        List<NetworkConnectionToClient> clientConnections = new List<NetworkConnectionToClient>();
        NetworkRoomManagerScript networkManager;
        GameObject playerCharacterPrefab;
        protected override void OnAwake()
        {
            _persistent = false;
        }

        void Awake()
        {
            networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
        }

        void Start()
        {

        }

        // Spawn
        public void spawnBullet(Transform transform)
        {
            GameObject bullet = Instantiate(networkManager.bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(transform.position + transform.forward, Vector3.up);
            bullet.GetComponent<NetworkedPaintBullet>().bulletSpeed = 50.0f;
            NetworkServer.Spawn(bullet);
        }

        public void spawnCharacter(NetworkConnectionToClient sender)
        {
            if (clientConnections.Contains(sender))
            {
                return;
            }
            clientConnections.Add(sender);
            GameObject playerCharacter = Instantiate(networkManager.playerCharacterPrefab);
            NetworkServer.Spawn(playerCharacter, sender);
        }
    }
}
