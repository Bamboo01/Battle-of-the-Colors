using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CSZZGame.Networking;

public class NetworkCharacter : NetworkBehaviour
{
    [SyncVar]
    public bool isShooting = false;

    [SyncVar]
    public bool isThrowing = false;

    [SyncVar] 
    public int ClientHP;

    [SyncVar]
    public float ClientSpeed;

    [SyncVar]
    public float ClientSkill1Cooldown;

    [SyncVar]
    public float ClientSkill2Cooldown;

    [SyncVar]
    public float ClientSkill3Cooldown;


    [SerializeField] Animator animator;
    [SerializeField] NetworkCharacterObserver observer;
    [SerializeField] NetworkCharacterController controller;
    [SerializeField] Transform firePoint;

    ServerCharacterData characterData;

    public override void OnStartServer()
    {
        characterData = gameObject.AddComponent<ServerCharacterData>();
    }

    void Start()
    {
        observer.animator = animator;
        observer.networkCharacter = this;
        observer.enabled = true;
        if (!hasAuthority)
        {
            return;
        }
        controller.enabled = true;
    }

    [ServerCallback]
    void Update()
    {
        ClientHP = characterData.HP;
        ClientSkill1Cooldown = characterData.Skill1Timer;
        ClientSkill2Cooldown = characterData.Skill2Timer;
        ClientSkill3Cooldown = characterData.Skill3Timer;
        ClientSpeed = characterData.Speed;
    }

    [Command]
    public void CmdFireBullet()
    {
        if (characterData.WeaponTimer <= 0)
        {
            characterData.weaponFired();
            CSZZNetworkServer.Instance.spawnBullet(firePoint);
        }
    }

    [Command]
    public void CmdSkill(int skillID)
    {
        if (skillID > 2)
        {
            return;
        }
    }


    // Debug
    [ClientRpc]
    public void DebugStringRPC(string str)
    {
        Debug.Log(str);
    }
}
