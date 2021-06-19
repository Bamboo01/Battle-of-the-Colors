using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo;

namespace SplatoonPainter
{
    [CreateAssetMenu(fileName = "new BrushProperties", menuName = "BrushProperties")]
    public class BrushProperties : ScriptableObject
    {
        [Header("Muzzle Flash Controls")]
        [Range(5, 1000)]
        [SerializeField] public int muzzleflashMinSpeed = 7;
        [Range(5, 1000)]
        [SerializeField] public int muzzleflashMaxSpeed = 15;
        [Range(40, 1000)]
        [SerializeField] public int muzzleflashEmissionAmount = 40;

        [Header("Bullet Shooter Particles Controls")]
        [Range(1, 60)]
        [SerializeField] public int particleSpawnRate = 5;
        [Range(1, 100)]
        [SerializeField] public int particleEmissionAmount = 1;
        [Range(0.001f, 10.0f)]
        [SerializeField] public float minRadius = 0.8f;
        [Range(0.001f, 10.0f)]
        [SerializeField] public float maxRadius = 1.1f;
        [Range(1, 100)]
        [SerializeField] public float minProjectileSpeed = 20.0f;
        [Range(1, 100)]
        [SerializeField] public float maxProjectileSpeed = 25.0f;

        [Header("Splash Controls")]
        [Range(5, 1000)]
        [SerializeField] public int splashMinSpeed = 7;
        [Range(5, 1000)]
        [SerializeField] public int splashMaxSpeed = 15;
        [Range(40, 1000)]
        [SerializeField] public int splashEmissionAmount = 10;
    }
}
