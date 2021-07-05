using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2ShieldBehaviour : NetworkBehaviour
{

    [SerializeField] Collider collider;

    [SerializeField] float duration = 15.0f;

    float spawnTime = 0;

    AudioSource loopSource = null;

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
        var a = SoundManager.Instance.PlaySoundAtPointByName("ShieldUp", transform.position);
        StartCoroutine(playLoop(a.clip.length));
    }

    void OnDestroy()
    {
        if (!isClient)
        {
            return;
        }
        if (loopSource != null)
        {
            loopSource.Stop();
            loopSource.gameObject.SetActive(false);
            loopSource = null;
        }
        SoundManager.Instance.PlaySoundAtPointByName("ShieldDown", transform.position);
        StopAllCoroutines();
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
            RPCHitShieldBullet();
        }
    }

    [ClientRpc]
    void RPCHitShieldBullet()
    {
        SoundManager.Instance.PlaySoundAtPointByName("ShieldHit", transform.position);
    }

    IEnumerator playLoop(float t)
    {
        yield return new WaitForSeconds(t);
        loopSource = SoundManager.Instance.PlaySoundAtPointByName("ShieldLoop", transform.position, false);
        yield break;
    }
}
