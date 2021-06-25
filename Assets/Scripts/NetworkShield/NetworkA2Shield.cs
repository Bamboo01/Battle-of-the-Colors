using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkA2Shield : NetworkBehaviour
{

    [SerializeField] Collider collider;

    [SerializeField] float duration = 15.0f;

    float spawnTime = 0;

    // The ACTUAL color of the particle
    private Color color;
    [Server]
    public void ServerSetup(Color c)
    {
        color = c;
    }

    [Server]
    void ServerUpdate()
    {
        spawnTime += Time.deltaTime;

        if (spawnTime > duration)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    public override void OnStartServer()
    {
        spawnTime = 0f;
    }

    public override void OnStartClient()
    {
        if (!isServer)
        {
            Destroy(collider);
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
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("hit shield");
        if (other.GetComponent<NetworkBullet>())
        {
            Debug.Log("deleto bullet");
            NetworkServer.Destroy(other.gameObject);
        }
    }
}
