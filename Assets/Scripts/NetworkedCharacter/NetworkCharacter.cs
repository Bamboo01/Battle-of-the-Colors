using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CSZZGame.Networking;
using CSZZGame.Refactor;

public class NetworkCharacter : NetworkBehaviour
{
    [SyncVar] 
    public int ClientHP;

    [SyncVar]
    public float ClientSpeed;

    readonly SyncDictionary<int, float> clientStrategemCooldowns = new SyncDictionary<int, float>();
    readonly SyncDictionary<int, bool> clientStrategemReady = new SyncDictionary<int, bool>();

    [SerializeField] NetworkCharacterController controller;
    [SerializeField] NetworkCharacterAnimator animator;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform lookAtPoint;

    // Server only
    NetworkRoomManagerScript networkManager;
    ServerCharacterData serverCharacterData;
    CSZZServerHandler server;

    public Behaviour[] SkillList;

    public override void OnStartServer()
    {
        networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
        SetupStrategems();
    }

    void Start()
    {
        if (!hasAuthority)
        {
            return;
        }
        // Setup of various local managers and callbacks related to changes done on the client's character
        StrategemManager.Instance.SetupUIManager(this, clientStrategemCooldowns);
        clientStrategemCooldowns.Callback += StrategemManager.Instance.OnStrategemUpdated;
        clientStrategemReady.Callback += StrategemManager.Instance.OnStrategemReady;

        controller.enabled = true;
        animator.enabled = true;
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
    public void SetupServerHandler(CSZZServerHandler s, ServerCharacterData characterData)
    {
        server = s;
        serverCharacterData = characterData;
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
            serverCharacterData.weaponFired();
            firePoint.rotation = lookAtPoint.rotation;
            server.spawnBullet(firePoint, serverCharacterData);
        }
    }

    [Command]
    public void CmdSpawnSkill(int skillID)
    {
        if (!networkManager.idToStrategem.ContainsKey(skillID))
        {
            return;
        }
        if (clientStrategemReady[skillID] == false)
        {
            return;
        }
        server.spawnStrategem(firePoint, serverCharacterData, skillID, this);
        serverCharacterData.strategemCooldowns[skillID] = networkManager.idToStrategem[skillID].cooldownTime;
        clientStrategemReady[skillID] = false;
    }
}
