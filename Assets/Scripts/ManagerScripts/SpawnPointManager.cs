using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;
using Bamboo.Utility;

public class SpawnPointManager : Singleton<SpawnPointManager>
{
    [SerializeField] public List<Transform> team1SpawnPoints;
    [SerializeField] public List<Transform> team2SpawnPoints;
}
