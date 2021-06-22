using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Utility;

// Handles calls from a client on the server side.
namespace CSZZGame.Networking
{
    public class CSZZServerHandler : MonoBehaviour
    {
        private NetworkConnectionToClient clientConnection;
        private NetworkRoomManagerScript networkManager;

        void Awake()
        {
            networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
        }

        public void spawnBullet(Transform transform, ServerCharacterData data)
        {
            GameObject bullet = Instantiate(networkManager.bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(transform.position + transform.forward, Vector3.up);

            NetworkBullet networkedPaintBullet = bullet.GetComponent<NetworkBullet>();

            networkedPaintBullet.bulletSpeed = 50.0f;
            networkedPaintBullet.ServerSetup(ServerCharacterData.teamToColor(data.characterTeam));
            NetworkServer.Spawn(bullet);
        }

        public void spawnCharacter(NetworkConnectionToClient sender)
        {
            if (clientConnection != null)
            {
                return;
            }
            clientConnection = sender;
            GameObject playerCharacter = Instantiate(networkManager.playerCharacterPrefab);
            playerCharacter.GetComponent<NetworkCharacter>().SetupServerHandler(this);
            NetworkServer.Spawn(playerCharacter, sender);
        }
    }
}
