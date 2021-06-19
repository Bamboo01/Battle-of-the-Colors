using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// CSZZ - 褰╄壊鎴樹簤
namespace CSZZGame.Networking
{
    public partial class NetworkPlayerScript : NetworkBehaviour
    {
        CharacterController controller;
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            if (!isLocalPlayer) return;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
        }
    }
}
