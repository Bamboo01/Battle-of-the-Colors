using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PaintBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10.0f;
    [SerializeField] float bulletGravity = 0.98f;
    [SerializeField] Rigidbody rb;

    public ParticlePainterProperties properties;
    public GameObject paintParticlesPrefab;

    void SpawnParticles()
    {
        GameObject go = Instantiate(paintParticlesPrefab, transform.position, transform.rotation);
        Destroy(go, 2.0f);
    }

    void Start()
    {
        rb.velocity = transform.forward * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        SpawnParticles();
        Destroy(gameObject);
    }
}