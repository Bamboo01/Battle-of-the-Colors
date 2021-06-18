using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Team", menuName = "Team")]
public class TeamProperties : ScriptableObject
{
    // For endgame calculation and brush color
    public Color teamColor;
    // For paintbrush bullets
    public Material BulletParticleMaterial;
    // Example...
    public Material ClotheMaterial;
}
