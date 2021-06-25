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

            networkedPaintBullet.ServerSetup(ServerCharacterData.teamToColor(data.characterTeam), 50.0f, 1.0f, 300.0f, 0.01f);
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

        public void spawnShield(Transform transform, ServerCharacterData data)
        {
            GameObject shield = Instantiate(networkManager.shieldPrefab);
            shield.transform.position = transform.position;

            Physics.Raycast(shield.transform.position + Vector3.up *3f, Vector3.down, out RaycastHit hit, 10f);
            shield.transform.position = hit.point;
            //shield.transform.LookAt(transform.position + transform.forward, Vector3.up);
            shield.transform.up = hit.normal;

            var localPos = shield.transform.InverseTransformDirection((transform.position + transform.forward) - shield.transform.position);
            localPos.y = 0;
            //var rotation = Quaternion.LookRotation(lookPos);

            Vector3 lookPos = shield.transform.position + shield.transform.TransformDirection(localPos);
            shield.transform.LookAt(lookPos, shield.transform.up);



            NetworkA2Shield networkedShield = shield.GetComponent<NetworkA2Shield>();
            networkedShield.ServerSetup(ServerCharacterData.teamToColor(data.characterTeam));
            NetworkServer.Spawn(shield);
        }

        public void spawnSkill(Transform transform, ISkill skill)
        {
            skill.UseSkill(transform, networkManager);
        }
    }
}
