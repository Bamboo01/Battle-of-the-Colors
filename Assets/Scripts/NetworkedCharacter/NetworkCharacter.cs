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

    ServerCharacterData serverCharacterData;
    CSZZServerHandler server;

    public Behaviour[] SkillList;

    public override void OnStartServer()
    {
        serverCharacterData = gameObject.AddComponent<ServerCharacterData>();
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

    void Update()
    {
        if (isServer)
        {
            ServerUpdate();
        }
    }

    void OnSkillReady()
    {

    }

    [Server]
    public void SetupServerHandler(CSZZServerHandler s)
    {
        server = s;
    }

    [Server]
    public void ServerUpdate()
    {
        ClientHP = serverCharacterData.HP;
        ClientSkill1Cooldown = serverCharacterData.Skill1Timer;
        ClientSkill2Cooldown = serverCharacterData.Skill2Timer;
        ClientSkill3Cooldown = serverCharacterData.Skill3Timer;
        ClientSpeed = serverCharacterData.Speed;
    }

    [Command]
    public void CmdFireBullet()
    {
        if (serverCharacterData.WeaponTimer <= 0)
        {
            isShooting = true;
            serverCharacterData.weaponFired();
            server.spawnBullet(firePoint, serverCharacterData);
        }
    }

    [Command]
    public void CmdSpawnSkill(int skillID)
    {
        if (skillID > 2)
        {
            return;
        }
        server.spawnSkill(firePoint, serverCharacterData, (ISkill)SkillList[skillID]);
    }
}
