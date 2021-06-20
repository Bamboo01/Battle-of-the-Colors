using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace CSZZGame.Networking
{
    [AddComponentMenu("")]
    public class NetworkPlayerScript : NetworkBehaviour
    {
        NetworkIdentity identity;
        NetworkServer server;

        public void Awake()
        {
            identity = GetComponent<NetworkIdentity>();
        }

        public override void OnStartServer()
        {
            server = gameObject.AddComponent<NetworkServer>();
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

