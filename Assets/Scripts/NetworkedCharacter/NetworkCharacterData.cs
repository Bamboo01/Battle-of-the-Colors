using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkCharacterData : NetworkBehaviour
{
    [SyncVar]
    int HP;
    [SyncVar]
    float Speed;
    [SyncVar]
    float Skill1CD;
    [SyncVar]
    float Skill2CD;
    [SyncVar]
    float Skill3CD;
}
