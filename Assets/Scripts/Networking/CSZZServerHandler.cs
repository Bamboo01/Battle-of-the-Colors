using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Utility;
using Bamboo.Events;
using CSZZGame.Refactor;

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

        void Start()
        {
        }

        public void spawnBullet(Transform transform, ServerCharacterData data)
        {
            GameObject bullet = Instantiate(networkManager.bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(transform.position + transform.forward, Vector3.up);

            NetworkBullet networkedPaintBullet = bullet.GetComponent<NetworkBullet>();

            networkedPaintBullet.ServerSetup(data.characterTeam, 50.0f, 1.0f, 300.0f, 0.01f);
            NetworkServer.Spawn(bullet);
        }

        public void spawnCharacter(NetworkConnectionToClient sender, ServerCharacterData characterData)
        {
            if (clientConnection != null)
            {
                return;
            }
            clientConnection = sender;
            GameObject playerCharacter = Instantiate(networkManager.playerCharacterPrefab);
            playerCharacter.GetComponent<NetworkCharacter>().SetupServerHandler(this, characterData);
            NetworkServer.Spawn(playerCharacter, sender);
        }

        public void spawnStrategem(Transform transform, ServerCharacterData data, int skillID, NetworkCharacter caller)
        {
            GameObject go = Instantiate(networkManager.idToStrategem[skillID].strategemPrefab);
            go.GetComponent<StrategemBase>().UseSkill(transform, data, this, networkManager, caller);
        }

        public void spawnSkill(Transform transform, ServerCharacterData data, StrategemBase skill, NetworkCharacter caller)
        {
            skill.UseSkill(transform, data, this, networkManager, caller);
        }

        public void RespawnCharacter(NetworkCharacter playercharacter)
        {
            Debug.Log("Server waiting to respawn in 5 seconds");
            StartCoroutine(_RespawnCharacter(playercharacter));
        }

        private IEnumerator _RespawnCharacter(NetworkCharacter playercharacter)
        {
            yield return new WaitForSeconds(5.0f);
            playercharacter.RespawnPlayerOnServer(Vector3.zero, Quaternion.identity);
            yield break;
        }
    }
}
