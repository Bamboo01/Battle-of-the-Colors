using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Character;

public struct SpawnInfo 
{
    public int SpawnIndex;
    public ServerCharacterData.CHARACTER_TEAM Team;

    public SpawnInfo(int a, ServerCharacterData.CHARACTER_TEAM b)
    {
        SpawnIndex = a;
        Team = b;
    }
}