using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Events;
using CSZZGame.Networking;
using CSZZGame.Refactor;

// Network interface for the player
namespace CSZZGame.Networking
{
    [AddComponentMenu("")]
    public class NetworkPlayerScript : NetworkBehaviour
    {
        [SerializeField] public ServerEventProperties serverEventProperties;
        [SerializeField] public ServerCharacterData characterData;
        protected CSZZServerHandler cszzNetworkServer;
        protected CSZZNetworkInterface cszzNetworkInterface;
        public bool isDedicatedServer = false;

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!isLocalPlayer && !isServer)
            {
                Destroy(characterData);
                return;
            }
            if (hasAuthority)
            {
                cszzNetworkInterface = gameObject.AddComponent<CSZZNetworkInterface>();
                cszzNetworkInterface.SetupClient(this);
                CmdCreatePlayerCharacter();
            }
        }

        [Command]
        public void CmdCreatePlayerCharacter(NetworkConnectionToClient sender = null)
        {
            cszzNetworkServer = gameObject.AddComponent<CSZZServerHandler>();
            cszzNetworkServer.ServerSetup();
            int index;
            cszzNetworkServer.spawnCharacter(sender, characterData, out index);
            OnTargetPlayerLoaded(sender, EventChannels.OnClientLoadedIntoGame, new SpawnInfo(index, characterData.characterTeam));
            EventManager.Instance.Publish(EventChannels.OnServerClientLoadedIntoGame, this);
        }

        [Command]
        public void CmdRaiseEvent(string eventChannel, byte[] data, NetworkConnectionToClient sender = null)
        {
            foreach (string n in serverEventProperties.ServerOnlyChannels)
            {
                if (eventChannel == n && (!isLocalPlayer || !isServer))
                {
                    Debug.LogError(sender.ToString() + " tried to raise a server only event: " + eventChannel);
                    return;
                }
            }    
            RaiseEventsToClients(eventChannel, data);
        }

        [ClientRpc]
        public void RaiseEventsToClients(string eventChannel, byte[] data)
        {
            EventManager.Instance.Publish(eventChannel, null, data);
        }

        [TargetRpc]
        public void RaiseEventToTarget(NetworkConnection target, string eventChannel, byte[] data)
        {
            EventManager.Instance.Publish(eventChannel, null, data);
        }

        [TargetRpc]
        public void OnTargetPlayerLoaded(NetworkConnection target, string eventChannel, SpawnInfo spawnIndex)
        {
            EventManager.Instance.Publish(eventChannel, null, spawnIndex);
        }
    }
}

