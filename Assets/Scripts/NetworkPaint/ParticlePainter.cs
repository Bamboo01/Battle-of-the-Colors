using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePainter : MonoBehaviour
{
    [Header("Particle Painter Properties")]
    [SerializeField] public ParticlePainterProperties particlePainterProperties;

    private ParticleSystem particleSystem;
    private ParticleSystemRenderer particleSystemRenderer;
    private ParticleSystem.MainModule particleSystemMain;
    private ParticleSystem.CollisionModule particleSystemCollider;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystemRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        particleSystemMain = particleSystem.main;
        particleSystemCollider = particleSystem.collision;
        if (!particleSystemCollider.enabled || !particleSystemCollider.sendCollisionMessages)
        {
            particleSystemCollider.enabled = true;
            particleSystemCollider.sendCollisionMessages = true;
        }
    }

    private void Start()
    {
        particleSystemMain.startSpeed = new ParticleSystem.MinMaxCurve(particlePainterProperties.particleMinStartSpeed, particlePainterProperties.particleMaxStartSpeed);
        particleSystemMain.gravityModifier = particlePainterProperties.particleGravity;
        particleSystemCollider.radiusScale = particlePainterProperties.particleColliderRadius;
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);
        Paintable p = other.GetComponent<Paintable>();
        if (p != null)
        {
            for (int i = 0; i < numCollisionEvents; i++)
            {
                // RPC to color stuff!
            }
        }
    }

    [ContextMenu("Reload Properties")]
    private void ReloadProperties()
    {
        particleSystemMain.startSpeed = new ParticleSystem.MinMaxCurve(particlePainterProperties.particleMinStartSpeed, particlePainterProperties.particleMaxStartSpeed);
        particleSystemMain.gravityModifier = particlePainterProperties.particleGravity;
        particleSystemCollider.radiusScale = particlePainterProperties.particleColliderRadius;
    }
}

