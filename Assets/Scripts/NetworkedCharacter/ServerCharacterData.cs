using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CSZZGame.Networking;

public class ServerCharacterData : MonoBehaviour
{
    public enum CHARACTER_TEAM
    {
        TEAM_1,
        TEAM_2,
        NUM_TEAMS
    }

    public static readonly Dictionary<CHARACTER_TEAM, Color> _teamToColor = new Dictionary<CHARACTER_TEAM, Color>()
    {
        { CHARACTER_TEAM.TEAM_1, new Color(1.0f, 0.8f, 0.1f ) },
        { CHARACTER_TEAM.TEAM_2, new Color(0.2f, 1.0f, 0.2f ) }
    };

    NetworkRoomManagerScript networkManager;

    public int HP { private set; get; }
    public float Speed { private set; get; }
    public Dictionary<int, float> strategemCooldowns = new Dictionary<int, float>();
    public float WeaponTimer { private set; get; }

    // Should be easy to change
    public float WeaponCD { private set; get; }

    public CHARACTER_TEAM characterTeam;
    public Color characterColor;

    void DefaultSetup()
    {
        WeaponCD = 0.15f;
        characterTeam = CHARACTER_TEAM.TEAM_1;
    }

    public void Respawn()
    {
        int[] strategemIDs = new int[strategemCooldowns.Count];
        strategemCooldowns.Keys.CopyTo(strategemIDs, 0);
        foreach (var id in strategemIDs)
        {
            strategemCooldowns[id] = networkManager.idToStrategem[id].cooldownTime;
        }
        HP = networkManager.maxPlayerHealth;
    }

    void Start()
    {
        networkManager = (NetworkRoomManagerScript)NetworkRoomManager.singleton;
        foreach (var strategem in networkManager.strategemProperties)
        {
            strategemCooldowns.Add(strategem.strategemID, strategem.cooldownTime);
        }
        HP = networkManager.maxPlayerHealth;
        WeaponCD = 0.15f;
    }

    void FixedUpdate()
    {
        List<int> idList = new List<int>(strategemCooldowns.Keys);
        foreach (int id in idList)
        {
            strategemCooldowns[id] = strategemCooldowns[id] - Time.fixedDeltaTime;
        }
        WeaponTimer -= Time.fixedDeltaTime;
    }

    public void swapTeam(bool team2)
    {
        characterTeam = team2 ? CHARACTER_TEAM.TEAM_2 : CHARACTER_TEAM.TEAM_1;
    }
    public void weaponFired()
    {
        WeaponTimer = WeaponCD;
    }

    public void playerDamaged(int damage)
    {
        HP -= damage;
    }

    public static Color teamToColor(CHARACTER_TEAM team)
    {
        return _teamToColor[team];
    }

    public static Color[] getAllTeamColors()
    {
        Color[] returnarray = new Color[_teamToColor.Values.Count];
        _teamToColor.Values.CopyTo(returnarray, 0);
        return returnarray;
    }
}
