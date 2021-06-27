using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Events;
using CSZZGame.Networking;
using CSZZGame.Refactor;

public class NetworkCharacter : NetworkBehaviour
{
    [SyncVar]
    [SerializeField] public int ClientHP;

    [SyncVar]
    [SerializeField]  public float ClientSpeed;

    [SyncVar] 
    [SerializeField] public ServerCharacterData.CHARACTER_TEAM team;

    readonly SyncDictionary<int, float> clientStrategemCooldowns = new SyncDictionary<int, float>();
    readonly SyncDictionary<int, bool> clientStrategemReady = new SyncDictionary<int, bool>();

    [SerializeField] NetworkCharacterController controller;
    [SerializeField] NetworkCharacterAnimator animator;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform lookAtPoint;
    [SerializeField] ParticleSystemRenderer psrenderer;
    // Server only
    NetworkRoomManagerScript networkManager;
    ServerCharacterData serverCharacterData;
    CSZZServerHandler server;

    public override void OnStartServer()
    {
        base.OnStartServer();
        networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
        SetupStrategems();
        team = serverCharacterData.characterTeam;
        // Subscribing to callbacks
        EventManager.Instance.Listen(EventChannels.OnServerGameEnd, OnServerEnd);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        EventManager.Instance.Publish(EventChannels.OnClientPlayerSpawn, this, team);
        psrenderer.material.SetColor("_BaseColor", ServerCharacterData.teamToColor(team));
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

    [Server]
    public void takeDamage(int amount, ServerCharacterData.CHARACTER_TEAM bulletTeam)
    {
        if (team != bulletTeam)
        {
            serverCharacterData.playerDamaged(amount);
        }
        if (serverCharacterData.HP <= 0)
        {
            RPCPOnCharacterDead(team);
            RPCToDeadPlayer(connectionToClient);
            server.RespawnCharacter(this);
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

    [ClientRpc]
    public void RPCPOnCharacterDead(ServerCharacterData.CHARACTER_TEAM team)
    {
        gameObject.SetActive(false);
        EventManager.Instance.Publish(EventChannels.OnClientPlayerDeath, this, team);
    }

    [TargetRpc]
    public void RPCToDeadPlayer(NetworkConnection target)
    {
        EventManager.Instance.Publish(EventChannels.OnTargetClientPlayerDeath, this);
    }

    [ClientRpc]
    public void RPCOnRespawnPlayer(ServerCharacterData.CHARACTER_TEAM team)
    {
        gameObject.SetActive(true);
        EventManager.Instance.Publish(EventChannels.OnClientPlayerSpawn, this, team);
    }

    [TargetRpc]
    public void RPCToRespawnedPlayer(NetworkConnection target)
    {
        EventManager.Instance.Publish(EventChannels.OnTargetClientPlayerSpawn, this);
    }


    [Server]
    public void RespawnPlayerOnServer(Vector3 respawnPosition, Quaternion rotation)
    {
        Debug.Log("Server preparing to respawn");
        transform.position = respawnPosition;
        transform.rotation = rotation;
        serverCharacterData.Respawn();
        RPCOnRespawnPlayer(team);
        RPCToRespawnedPlayer(connectionToClient);
    }

    [Server]
    void OnServerEnd(IEventRequestInfo info)
    {
        server.ServerOnGameEnd(this);
    }

    //void TeamSet(ServerCharacterData.CHARACTER_TEAM oldteam, ServerCharacterData.CHARACTER_TEAM newteam)
    //{
    //    EventManager.Instance.Publish(EventChannels.OnClientPlayerSpawn, this, newteam);
    //}

    #region Debug
    [ContextMenu("Force Kill 1")]
    [Server]
    void forceDieTeam1()
    {
        takeDamage(1000000, ServerCharacterData.CHARACTER_TEAM.TEAM_2);
    }

    [ContextMenu("Force Kill 2")]
    [Server]
    void forceDieTeam2()
    {
        takeDamage(1000000, ServerCharacterData.CHARACTER_TEAM.TEAM_1);
    }
    #endregion
}
