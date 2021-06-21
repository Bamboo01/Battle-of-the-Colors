using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerCharacterData : MonoBehaviour
{
    public int HP { private set; get; }
    public float Speed { private set; get; }
    public float Skill1Timer { private set; get; }
    public float Skill2Timer { private set; get; }
    public float Skill3Timer { private set; get; }
    public float WeaponTimer { private set; get; }

    // Should be easy to change
    public float Skill1CD { private set; get; }
    public float Skill2CD { private set; get; }
    public float Skill3CD { private set; get; }
    public float WeaponCD { private set; get; }

    void DefaultSetup()
    {
        Skill1CD = 5.0f;
        Skill2CD = 5.0f;
        Skill3CD = 5.0f;
        WeaponCD = 0.5f;
    }

    void Start()
    {
        DefaultSetup();
    }

    void FixedUpdate()
    {
        Skill1Timer -= Time.fixedDeltaTime;
        Skill2Timer -= Time.fixedDeltaTime;
        Skill3Timer -= Time.fixedDeltaTime;
        WeaponTimer -= Time.fixedDeltaTime;
    }

    public void weaponFired()
    {
        WeaponTimer = WeaponCD;
    }
}
