using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CSZZGame.Networking;
using CSZZGame.Refactor;

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

    readonly SyncDictionary<int, float> clientStrategemCooldowns = new SyncDictionary<int, float>();
    readonly SyncDictionary<int, bool> clientStrategemReady = new SyncDictionary<int, bool>();

    [SerializeField] Animator animator;
    [SerializeField] NetworkCharacterObserver observer;
    [SerializeField] NetworkCharacterController controller;
    [SerializeField] Transform firePoint;

    // Server only
    NetworkRoomManagerScript networkManager;
    ServerCharacterData serverCharacterData;
    CSZZServerHandler server;

    public Behaviour[] SkillList;

    public override void OnStartServer()
    {
        serverCharacterData = gameObject.AddComponent<ServerCharacterData>();
        networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
        SetupStrategems();
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
        // Setup of various local managers and callbacks related to changes done on the client's character
        UIManager.Instance.SetupUIManager(this, clientStrategemCooldowns);
        clientStrategemCooldowns.Callback += UIManager.Instance.OnStrategemUpdated;
        clientStrategemReady.Callback += UIManager.Instance.OnStrategemReady;

        controller.enabled = true;
    }

    void Update()
    {
        if (isServer)
        {
            ServerUpdate();
        }
    }

    [Server]
    public void SetupStrategems()
    {
        // I think most games would allow you to choose what skills to add
        //
        // Since I have no time I'm just gonna add all of them, but this is just
        // here for the sake of functionality in case I ever have to lay hands on
        // this project again
        
        foreach (var strategem in networkManager.strategemProperties)
        {
            clientStrategemCooldowns.Add(strategem.strategemID, strategem.cooldownTime);
            clientStrategemReady.Add(strategem.strategemID, false);
            serverCharacterData.strategemCooldowns.Add(strategem.strategemID, strategem.cooldownTime);
        }
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
        ClientSpeed = serverCharacterData.Speed;

        foreach (var pair in serverCharacterData.strategemCooldowns)
        {
            clientStrategemCooldowns[pair.Key] = pair.Value;
            if (clientStrategemReady[pair.Key] == false && pair.Value <= 0.0f)
            {
                clientStrategemReady[pair.Key] = true;
            }
        }
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
