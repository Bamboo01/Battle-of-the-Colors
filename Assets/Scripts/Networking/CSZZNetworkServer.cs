using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Utility;

// Handles calls from a client on the server side.
namespace CSZZGame.Networking
{
    public class CSZZNetworkServer : MonoBehaviour
    {
        private List<NetworkConnectionToClient> clientConnections = new List<NetworkConnectionToClient>();
        private NetworkRoomManagerScript networkManager;

        void Awake()
        {
            networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
        }

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
            playerCharacter.GetComponent<NetworkCharacter>().SetupServer(this);
            NetworkServer.Spawn(playerCharacter, sender);
        }
    }
}
