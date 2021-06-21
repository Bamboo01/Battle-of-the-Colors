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
        CSZZNetworkServer server;

        public void Awake()
        {
            identity = GetComponent<NetworkIdentity>();
        }

        public override void OnStartServer()
        {
            server = gameObject.AddComponent<CSZZNetworkServer>();
        }

        public void Start()
        {
            CreatePlayerCharacter(identity.connectionToClient);
        }

        [Command]
        public void CreatePlayerCharacter(NetworkConnectionToClient sender)
        {
            server.spawnCharacter(sender);
        }
    }
}

