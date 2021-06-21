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
        private CSZZNetworkServer cszzNetworkServer;
        private CSZZNetwork cszzNetwork;
        public override void OnStartServer()
        {
            if (!isLocalPlayer)
            {
                return;
            }    
            base.OnStartServer();
            cszzNetworkServer = gameObject.AddComponent<CSZZNetworkServer>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!isLocalPlayer)
            {
                return;
            }
            cszzNetwork = gameObject.AddComponent<CSZZNetwork>();
            cszzNetwork.SetupClient(this);
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
        public void CmdRaiseEvent(string eventChannel, byte[] data)
        {
            OnEventRaised(eventChannel, data);
        }

        [ClientRpc]
        public void OnEventRaised(string eventChannel, byte[] data)
        {
            EventManager.Instance.Publish(eventChannel, null, data);
        }
    }
}

