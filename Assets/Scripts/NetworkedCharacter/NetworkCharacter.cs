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
    [SerializeField] public float ClientSpeed;

    [SyncVar]
    [SerializeField] public ServerCharacterData.CHARACTER_TEAM team;

    readonly SyncDictionary<int, float> clientStrategemCooldowns = new SyncDictionary<int, float>();
    readonly SyncDictionary<int, bool> clientStrategemReady = new SyncDictionary<int, bool>();

    [SerializeField] public NetworkCharacterController controller;
    [SerializeField] NetworkCharacterAnimator animator;
    [SerializeField] NetworkCharacterSoundController soundController;
    public NetworkTransform networkTransform;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform lookAtPoint;
    [SerializeField] ParticleSystemRenderer psrenderer;
    [SerializeField] Renderer[] playerRenderers;


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
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        EventManager.Instance.Publish(EventChannels.OnClientPlayerSpawn, this, team);
        psrenderer.material.SetColor("_BaseColor", ServerCharacterData.teamToColor(team));
    }

    void Start()
    {
        foreach (var a in playerRenderers)
        {
            a.material.SetFloat("_CamDistThreshold", 0.0f);
        }
        networkTransform = GetComponent<NetworkTransform>();
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
    public void ResetStrategems()
    {
        foreach (var a in serverCharacterData.strategemCooldowns)
        {
            clientStrategemCooldowns[a.Key] = a.Value;
            clientStrategemReady[a.Key] = false;
        }
    }


    [Server]
    public void takeDamage(NetworkConnection shooter, int amount, ServerCharacterData.CHARACTER_TEAM bulletTeam)
    {
        if (team != bulletTeam)
        {
            serverCharacterData.playerDamaged(amount);
            RPCTargetHitTarget(shooter);
        }
        if (serverCharacterData.HP <= 0)
        {
            gameObject.SetActive(false);
            server.RespawnCharacter();
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
            RPCCLientShot(transform.position);
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
    public void RPCCLientShot(Vector3 position)
    {
        SoundManager.Instance.PlaySoundAtPointByName("Machinegun" + Random.Range(0, 3).ToString(), position);
    }

    [ClientRpc]
    public void RPCPOnCharacterDead(ServerCharacterData.CHARACTER_TEAM team)
    {
        gameObject.SetActive(false);

        SoundManager.Instance.PlaySoundAtPointByName("Die", transform.position);

        GameObject particlesGameObject = ObjectPool.Instance.spawnFromPool("PaintBulletParticles");
        particlesGameObject.transform.position = transform.position;
        particlesGameObject.transform.rotation = transform.rotation;
        particlesGameObject.GetComponent<BulletPaintParticlesManager>().SetupPaintParticles(ServerCharacterData.teamToColor(team), 3.0f);
        particlesGameObject.SetActiveDelayed(false, 3.0f);

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
        gameObject.SetActive(true);
        controller.enabled = true;
        animator.enabled = true;
        EventManager.Instance.Publish(EventChannels.OnTargetClientPlayerSpawn, this);
    }

    [TargetRpc]
    public void RPCTargetHitTarget(NetworkConnection target)
    {
        EventManager.Instance.Publish(EventChannels.OnTargetClientHitSomething, this);
    }

    [TargetRpc]
    public void RPCOnReadyTargettedPlayer()
    {
        controller.enabled = true;
    }


    [Server]
    public void RespawnPlayerOnServer()
    {
        Debug.Log("Server preparing to respawn");
        ResetStrategems();
        gameObject.SetActive(true);
        serverCharacterData.Respawn();
        RPCOnRespawnPlayer(team);
        RPCToRespawnedPlayer(connectionToClient);
    }

    [TargetRpc]
    public void TargetClientGameStarted(NetworkConnection connection)
    {
        foreach (var r in playerRenderers)
        {
            var teammateOutliner = r.gameObject.GetComponent<Outline>();
            teammateOutliner.enabled = false;
        }
        controller.enabled = true;
        animator.enabled = true;

        // Setup of various local managers and callbacks related to changes done on the client's character
        StrategemManager.Instance.SetupUIManager(this, clientStrategemCooldowns);
        clientStrategemCooldowns.Callback += StrategemManager.Instance.OnStrategemUpdated;
        clientStrategemReady.Callback += StrategemManager.Instance.OnStrategemReady;
    }

    [ClientRpc]
    public void RPCGameStarted()
    {
        if (team == (GameController.localRoomPlayer.team ? ServerCharacterData.CHARACTER_TEAM.TEAM_2 : ServerCharacterData.CHARACTER_TEAM.TEAM_1))
        {
            foreach(var r in playerRenderers)
            {
                var teammateOutliner = r.gameObject.GetComponent<Outline>();
                teammateOutliner.OutlineColor = ServerCharacterData.teamToColor(team);
                teammateOutliner.enabled = true;
            }
        }

        foreach (var a in playerRenderers)
        {
            a.material.SetFloat("_CamDistThreshold", 8.0f);
        }
        soundController.enabled = true;
    }

    #region Debug
    [ContextMenu("Force Kill 1")]
    [Server]
    void forceDieTeam1()
    {
        takeDamage(netIdentity.connectionToClient, 1000000, ServerCharacterData.CHARACTER_TEAM.TEAM_2);
    }

    [ContextMenu("Force Kill 2")]
    [Server]
    void forceDieTeam2()
    {
        takeDamage(netIdentity.connectionToClient, 1000000, ServerCharacterData.CHARACTER_TEAM.TEAM_1);
    }
    #endregion
}
