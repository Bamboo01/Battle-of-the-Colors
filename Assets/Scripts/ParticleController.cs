using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [HideInInspector] public int particleEmissionAmount;

    [HideInInspector] public float resetTime;
    [HideInInspector] float firetimer = 0.0f;
    [HideInInspector] public float minRadius;
    [HideInInspector] public float maxRadius;

    [SerializeField] public SimplePaintBrush brush = new SimplePaintBrush();
    [SerializeField] public ParticleSystem particleshooter;
    [SerializeField] public ParticleSystem muzzleflash;
    [SerializeField] public ParticleSystem splash;

    [SerializeField] public ParticleSystemRenderer particleshooter_psr;
    [SerializeField] public ParticleSystemRenderer muzzleflash_psr;
    [SerializeField] public ParticleSystemRenderer splash_psr;

    [HideInInspector] List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void FixedUpdate()
    {
        firetimer -= Time.fixedDeltaTime;
    }

    public void Shoot(IEventRequestInfo info)
    {
        if (firetimer > 0) return;
        muzzleflash.Emit(40);
        particleshooter.Emit(particleEmissionAmount);
        muzzleflash.Play();
        particleshooter.Play();
        muzzleflash.Stop();
        particleshooter.Stop();
        firetimer = resetTime;
    }

    public void StopShoot(IEventRequestInfo info)
    {
        muzzleflash.Stop();
        particleshooter.Stop();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particleshooter.GetCollisionEvents(other, collisionEvents);
        Paintable p = other.GetComponent<Paintable>();
        if (p != null)
        {
            for (int i = 0; i < numCollisionEvents; i++)
            {
                Vector3 pos = collisionEvents[i].intersection;
                float radius = Random.Range(minRadius, maxRadius);
                brush.position = pos;
                brush.radius = radius;
                PaintManager.instance.Paint(p, ref brush);
            }
        }
    }
}
