using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerCharacterData : MonoBehaviour
{
    public enum CHARACTER_TEAM
    {
        TEAM_1,
        TEAM_2,
        NUM_TEAMS
    }

    private static readonly Dictionary<CHARACTER_TEAM, Color> _teamToColor = new Dictionary<CHARACTER_TEAM, Color>()
    {
        { CHARACTER_TEAM.TEAM_1, new Color(1.0f, 0.8f, 0.1f ) },
        { CHARACTER_TEAM.TEAM_2, new Color(0.2f, 1.0f, 0.2f ) }
    };


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
        WeaponCD = 0.5f;
        characterTeam = CHARACTER_TEAM.TEAM_1;
    }

    void Start()
    {
        DefaultSetup();
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

    public static Color teamToColor(CHARACTER_TEAM team)
    {
        return _teamToColor[team];
    }
}
