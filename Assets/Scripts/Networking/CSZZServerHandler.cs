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
        private NetworkCharacter networkCharacter;

        [Server]
        public void ServerSetup()
        {
            networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
            EventManager.Instance.Listen(EventChannels.OnServerGameStarted, OnServerGameStarted);
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

        public void spawnCharacter(NetworkConnectionToClient sender, ServerCharacterData characterData, out int spawnIndex)
        {
            List<Transform> spawnPoints = networkManager.teamToStartPositionList[characterData.characterTeam];
            int index = Random.Range(0, spawnPoints.Count);
            spawnIndex = index;
            if (clientConnection != null)
            {
                return;
            }
            clientConnection = sender;

            GameObject playerCharacter = Instantiate(networkManager.playerCharacterPrefab, spawnPoints[index].position, spawnPoints[index].rotation);
            networkCharacter = playerCharacter.GetComponent<NetworkCharacter>();
            networkCharacter.SetupServerHandler(this, characterData);
            networkCharacter.transform.position = spawnPoints[index].position;
            networkCharacter.transform.rotation = spawnPoints[index].rotation;
            NetworkServer.Spawn(playerCharacter);
        }

        public void AssignAuthority()
        {
            if (clientConnection != null)
            {
                networkCharacter.netIdentity.AssignClientAuthority(clientConnection);
                networkCharacter.networkTransform.clientAuthority = true;
            }
        }

        public void RevokeAuthority()
        {
            if (clientConnection != null)
            {
                networkCharacter.netIdentity.RemoveClientAuthority();
            }
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

        public void RespawnCharacter()
        {
            Debug.Log("Server waiting to respawn in 5 seconds");
            StartCoroutine(_RespawnCharacter(networkCharacter));
        }

        private IEnumerator _RespawnCharacter(NetworkCharacter playercharacter, float time = 5.0f)
        {
            playercharacter.RPCToDeadPlayer(clientConnection);
            RevokeAuthority();
            playercharacter.RPCPOnCharacterDead(playercharacter.team);
            List<Transform> spawnPoints = networkManager.teamToStartPositionList[playercharacter.team];
            int index = Random.Range(0, spawnPoints.Count);
            playercharacter.networkTransform.ServerTeleport(spawnPoints[index].position, spawnPoints[index].rotation);
            yield return new WaitForSeconds(time);
            AssignAuthority();
            playercharacter.gameObject.SetActive(true);
            playercharacter.RespawnPlayerOnServer();
            yield break;
        }

        [Server]
        public void ServerOnGameEnd(NetworkCharacter playercharacter)
        {
            RevokeAuthority();
        }

        [Server]
        public void OnServerGameStarted(IEventRequestInfo info)
        {
            // Assign authority at end of the countdown!
            AssignAuthority();
            networkCharacter.RPCGameStarted();
            networkCharacter.TargetClientGameStarted(clientConnection);
        }
    }
}
