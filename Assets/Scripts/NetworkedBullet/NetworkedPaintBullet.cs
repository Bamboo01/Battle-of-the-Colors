using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkedPaintBullet : NetworkBehaviour
{
    [SerializeField] public float bulletSpeed = 10.0f;
    [SerializeField] Renderer renderer;
    [SerializeField] Collider collider;
    [SerializeField] Rigidbody rb;
    public ParticlePainterProperties properties;
    public GameObject paintParticlesPrefab;
    public float masDistanceTravelled = 300.0f;

    private Vector3 lastPosition;

    [ClientRpc]
    void SpawnParticlesRPC(Vector3 contactPoint)
    {
        Debug.Log("Particles spawned");
        GameObject go = Instantiate(paintParticlesPrefab, contactPoint, transform.rotation);
        Destroy(go, 2.0f);
        Destroy(gameObject);
    }

    void Start()
    {
        if (!isServer)
        {
            Destroy(collider);
            Destroy(rb);
        }
        lastPosition = transform.position;
        rb.velocity = transform.forward * bulletSpeed;
    }

    [Server]
    void Update()
    {
        masDistanceTravelled += (lastPosition - transform.position).magnitude;
        lastPosition = transform.position;
    }

    [Server]
    void OnCollisionEnter(Collision collision)
    {
        renderer.enabled = false;
        Destroy(collider);
        Destroy(rb);
        SpawnParticlesRPC(transform.position);
    }
}
