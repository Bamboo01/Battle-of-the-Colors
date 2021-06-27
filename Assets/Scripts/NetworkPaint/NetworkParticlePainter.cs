using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;
using CSZZGame.Networking;

[System.Serializable]
struct PaintParticlesNetworkData
{
    public ParticlePainterPropertiesSerialized painterProperties;
    public SerializedVector4[] collisionPositions;
    public SerializedVector4 actualColor;
    public int paintableID;
    public float paintScale;

    public PaintParticlesNetworkData(ParticlePainterProperties props, List<Vector3> pos, int id, Color color, float scale)
    {
        painterProperties = new ParticlePainterPropertiesSerialized(props);
        collisionPositions = SerializedVector4.vec3ArrayToSerializedVec4(pos.ToArray());
        paintableID = id;
        actualColor = new SerializedVector4(color);
        paintScale = scale;
    }

    public PaintParticlesNetworkData(ParticlePainterProperties props, Vector3 pos, int id, Color color, float scale)
    {
        painterProperties = new ParticlePainterPropertiesSerialized(props);
        collisionPositions = new SerializedVector4[1] { new SerializedVector4(pos) };
        paintableID = id;
        actualColor = new SerializedVector4(color);
        paintScale = scale;
    }
}

[RequireComponent(typeof(ParticleSystem))]
public class NetworkParticlePainter : MonoBehaviour
{

    [HideInInspector] public ParticlePainterProperties particlePainterProperties;

    Color actualColor;
    private ParticleSystem particleSystem;
    private ParticleSystemRenderer particleSystemRenderer;
    private ParticleSystem.MainModule particleSystemMain;
    private ParticleSystem.CollisionModule particleSystemCollider;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    [HideInInspector] public float paintScaleModifier = 1.0f;

    public float localPaintScaleModifier = 1.0f;

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

    public void SetupPainter(Color color, float paintscale = 1.0f)
    {
        paintScaleModifier = paintscale * localPaintScaleModifier;
        actualColor = color;
        particleSystemMain.startSpeed = new ParticleSystem.MinMaxCurve(particlePainterProperties.particleMinStartSpeed, particlePainterProperties.particleMaxStartSpeed);
        particleSystemMain.gravityModifier = particlePainterProperties.particleGravity;
        particleSystemCollider.radiusScale = particlePainterProperties.particleColliderRadius * paintscale;
    }

    void OnParticleCollision(GameObject other)
    {
        List<Vector3> impactPositions = new List<Vector3>();
        int hitPaintableID = -1;
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);
        Paintable p = other.GetComponent<Paintable>();
        if (p != null)
        {
            hitPaintableID = NetworkPainterManager.Instance.GetPaintableID(p);
            for (int i = 0; i < numCollisionEvents; i++)
            {
                impactPositions.Add(collisionEvents[i].intersection);
            }
        }

        if (hitPaintableID != -1)
        {
            CSZZNetworkInterface.Instance.SendNetworkEvent(EventChannels.OnPaintPaintableEvent, new PaintParticlesNetworkData(particlePainterProperties, impactPositions, hitPaintableID, actualColor, paintScaleModifier));
        }
    }
}

