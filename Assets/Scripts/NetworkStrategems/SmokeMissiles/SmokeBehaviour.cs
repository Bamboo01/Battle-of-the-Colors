using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBehaviour : NetworkBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] private GameObject smokeEffectPrefab;
    [SerializeField] private float smokeDuration = 8f;

    public override void OnStartClient()
    {
        if (!isServer)
        {
            Destroy(collider);
        }
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("spawning smoke on clients");
        RPCeffects();
        Destroy(gameObject, 5f);
    }


    [ClientRpc]
    private void RPCeffects()
    {
        Destroy(Instantiate(smokeEffectPrefab, transform.position, smokeEffectPrefab.transform.rotation), smokeDuration);
    }
}
