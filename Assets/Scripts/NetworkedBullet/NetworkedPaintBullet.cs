using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkedPaintBullet : NetworkBehaviour
{
    [SerializeField] public float bulletSpeed = 10.0f;

    public ParticlePainterProperties properties;
    public GameObject paintParticlesPrefab;
    Rigidbody rb;

    [ClientRpc]
    void SpawnParticlesRPC()
    {
        GameObject go = Instantiate(paintParticlesPrefab, transform.position, transform.rotation);
        Destroy(go, 2.0f);
    }

    [ServerCallback]
    void Start()
    {
        if (!isServer)
        {
            Destroy(GetComponent<Rigidbody>());
        }
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
    }

    [ServerCallback]
    void OnCollisionEnter(Collision collision)
    {
        SpawnParticlesRPC();
        Destroy(gameObject);
    }
}
