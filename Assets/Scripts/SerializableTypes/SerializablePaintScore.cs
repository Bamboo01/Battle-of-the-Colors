using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializablePaintScore
{
    public int team1Score;
    public int team2Score;

    public SerializablePaintScore(int team1, int team2)
    {
        team1Score = team1;
        team2Score = team2;
    }
}
