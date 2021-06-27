using CSZZGame.Networking;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintMissileBehaviour : NetworkBehaviour
{
    [SerializeField] private Collider collider;
    public CSZZServerHandler server { get; set; }
    public ServerCharacterData serverCharacterData { get; set; }

    private bool triggered = false;
    public override void OnStartClient()
    {
        if (!isServer)
        {
            Destroy(collider);
        }
        triggered = false;
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if(triggered || other.GetComponent<PaintMissileBehaviour>())
        {
            return;
        }
        triggered = true;
        Transform newTransform = transform;
        newTransform.position += Vector3.up;
        newTransform.forward = Vector3.down;
        newTransform.localScale = newTransform.localScale;
        server.spawnBullet(newTransform, serverCharacterData, 4.0f);
        Destroy(gameObject);
    }
}