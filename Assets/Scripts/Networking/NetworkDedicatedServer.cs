using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Networking;

namespace CSZZGame.Networking
{
    public class NetworkDedicatedServer : NetworkPlayerScript
    {
        public override void OnStartServer()
        {
            isDedicatedServer = true;
            cszzNetworkInterface = gameObject.AddComponent<CSZZNetworkInterface>();
            cszzNetworkInterface.SetupClient(this);
        }

        public override void OnStartClient()
        {
        }

        public override void OnStartAuthority()
        {
        }

        void OnDestroy()
        {
            Debug.Log("Dedicated Server stopped...");
        }

        void Start()
        {
            
        }
    }
}
