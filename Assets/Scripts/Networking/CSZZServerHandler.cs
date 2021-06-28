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

        public void spawnBullet(Transform transform, ServerCharacterData data, float scale = 1.0f)
        {
            GameObject bullet = Instantiate(networkManager.bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(transform.position + transform.forward, Vector3.up);
            bullet.transform.localScale = new Vector3(scale, scale, scale);
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

            List<Transform> spawnPoints = networkManager.teamToStartPositionList[characterData.characterTeam];
            int index = Random.Range(0, spawnPoints.Count);
            GameObject playerCharacter = Instantiate(networkManager.playerCharacterPrefab, spawnPoints[index].position, spawnPoints[index].rotation);
            var networkcharacter = playerCharacter.GetComponent<NetworkCharacter>();
            networkcharacter.SetupServerHandler(this, characterData);
            networkcharacter.respawnedPosition = spawnPoints[index].position;
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
            //playercharacter.netIdentity.RemoveClientAuthority();
            StartCoroutine(_RespawnCharacter(playercharacter));
        }

        private IEnumerator _RespawnCharacter(NetworkCharacter playercharacter, float time = 5.0f)
        {
            yield return new WaitForSeconds(time);
            List<Transform> spawnPoints = networkManager.teamToStartPositionList[playercharacter.team];
            int index = Random.Range(0, spawnPoints.Count);
            playercharacter.respawnedPosition = spawnPoints[index].position;
            playercharacter.transform.position = spawnPoints[index].position;
            playercharacter.RespawnPlayerOnServer(spawnPoints[index].position, spawnPoints[index].rotation);
            //playercharacter.netIdentity.AssignClientAuthority(clientConnection);
            yield break;
        }

        public void ServerOnGameEnd(NetworkCharacter playercharacter)
        {
            playercharacter.netIdentity.RemoveClientAuthority();
        }
    }
}
