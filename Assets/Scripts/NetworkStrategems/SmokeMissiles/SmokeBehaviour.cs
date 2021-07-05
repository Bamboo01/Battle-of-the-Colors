using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBehaviour : NetworkBehaviour
{
    [SerializeField] private Collider collider;
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
        GameObject particlesGameObject = ObjectPool.Instance.spawnFromPool("SmokeEffect");
        particlesGameObject.transform.position = transform.position;
        particlesGameObject.transform.rotation = transform.rotation;
        particlesGameObject.SetActiveDelayed(false, smokeDuration);

        AudioSource pointSound = SoundManager.Instance.PlaySoundAtPointByName("SmokeBombHit", transform.position);
    }
}
