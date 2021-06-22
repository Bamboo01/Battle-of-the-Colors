using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkBullet : NetworkBehaviour
{
    [SerializeField] public float bulletSpeed = 10.0f;
    [SerializeField] Renderer renderer;
    [SerializeField] Collider collider;
    [SerializeField] Rigidbody rb;
    // Properties for the particles
    public ParticlePainterProperties properties;
    // The ACTUAL color of the particle
    public Color color;
    // Particles that will be RPC'd (dummy particles)
    public GameObject paintParticlesPrefab;
    // Particles that will only be spawned on the server
    public GameObject brushParticlesPrefab;
    public float masDistanceTravelled = 300.0f;

    private Vector3 lastPosition;

    [Server]
    public void ServerSetup(Color c)
    {
        color = c;
    }

    [Server]
    void ServerUpdate()
    {
        masDistanceTravelled += (lastPosition - transform.position).magnitude;
        lastPosition = transform.position;
    }

    public override void OnStartServer()
    {
        lastPosition = transform.position;
        rb.velocity = transform.forward * bulletSpeed;
    }

    public override void OnStartClient()
    {
        if (!isServer)
        {
            Destroy(collider);
            Destroy(rb);
        }
    }

    void Update()
    {
        if (isServer)
        {
            ServerUpdate();
        }
    }

    [Server]
    void OnCollisionEnter(Collision collision)
    {
        renderer.enabled = false;
        Destroy(collider);
        Destroy(rb);

        Vector3 dir = -collision.contacts[0].normal;
        Vector3 newForwardDir = Vector3.Cross(transform.right, dir);
        Vector3 lookPos = transform.position + newForwardDir.normalized;

        // Spawn the particles locally (server)
        GameObject brushParticles = Instantiate(brushParticlesPrefab, transform.position, transform.rotation);
        brushParticles.GetComponent<BulletBrushParticlesManager>().SetupServerParticles(properties);
        brushParticles.transform.LookAt(lookPos, -dir);
        Destroy(brushParticles, 3.0f);

        SpawnParticlesRPC(transform.position, lookPos, -dir);
    }


    [ClientRpc]
    void SpawnParticlesRPC(Vector3 contactPoint, Vector3 target, Vector3 up)
    {
        Debug.Log("Particles spawned");
        GameObject particlesGameObject = Instantiate(paintParticlesPrefab, contactPoint, transform.rotation);
        particlesGameObject.transform.LookAt(target, up);
        Destroy(particlesGameObject, 3.0f);
        Destroy(gameObject);
    }
}
