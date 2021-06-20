using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkCharacter : NetworkBehaviour
{
    [SyncVar]
    public bool isShooting = false;

    [SyncVar]
    public bool isThrowing = false;

    [SerializeField] Animator animator;
    [SerializeField] NetworkCharacterObserver observer;
    [SerializeField] NetworkCharacterController controller;
    void Start()
    {
        if (!hasAuthority)
        {
            observer.animator = animator;
            observer.networkCharacter = this;
            observer.enabled = true;
            return;
        }
        controller.enabled = true;
    }
}
