using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Events;

// Network interface for the player
namespace CSZZGame.Networking
{
    [AddComponentMenu("")]
    public class NetworkPlayerScript : NetworkBehaviour
    {
        [SerializeField] public ServerEventProperties serverEventProperties;
        protected CSZZServerHandler cszzNetworkServer;
        protected CSZZNetworkInterface cszzNetworkInterface;
        public bool isDedicatedServer = false;

        public override void OnStartServer()
        {
            cszzNetworkServer = gameObject.AddComponent<CSZZServerHandler>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!isLocalPlayer)
            {
                return;
            }
            cszzNetworkInterface = gameObject.AddComponent<CSZZNetworkInterface>();
            cszzNetworkInterface.SetupClient(this);
        }

        public override void OnStartAuthority()
        {
            CmdCreatePlayerCharacter();
        }

        [Command]
        public void CmdCreatePlayerCharacter(NetworkConnectionToClient sender = null)
        {
            cszzNetworkServer.spawnCharacter(sender);
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
        public void RaiseEventsToTargettedClient(NetworkConnection target, string eventChannel, byte[] data)
        {
            EventManager.Instance.Publish(eventChannel, null, data);
        }
    }
}

