using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace CSZZGame.Networking
{
    [AddComponentMenu("")]
    public class NetworkPlayerScript : NetworkBehaviour
    {
        [SyncVar] int clientTick;

        NetworkIdentity identity;

        public void Awake()
        {
            identity = GetComponent<NetworkIdentity>();
        }

        public override void OnStartServer()
        {
            if (!isLocalPlayer)
            {
                return;
            }    
            base.OnStartServer();
            gameObject.AddComponent<CSZZNetworkServer>();
        }

        public override void OnStartAuthority()
        {
            CreatePlayerCharacter(identity.connectionToClient);
        }

        [Command]
        public void CreatePlayerCharacter(NetworkConnectionToClient sender)
        {
            CSZZNetworkServer.Instance.spawnCharacter(sender);
        }
    }
}

