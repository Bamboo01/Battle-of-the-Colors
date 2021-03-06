using CSZZGame.Networking;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMissile : StrategemBase
{
    float missleCount = 8;
    float startupDelay = 0.5f;
    float missleDelay = 0.1f;
    [SerializeField] float force = 30f;

    public override void UseSkill(Transform startPoint, ServerCharacterData data, CSZZServerHandler serverHandler, NetworkRoomManagerScript server, NetworkCharacter caller)
    {
        StartCoroutine(SpawnMissiles(startPoint, server));
    }

    IEnumerator SpawnMissiles(Transform startPoint, NetworkRoomManagerScript server)
    {
        GameObject airdropMarker = Instantiate(server.markerPrefab, startPoint.position, server.markerPrefab.transform.rotation);
        airdropMarker.GetComponent<Rigidbody>().AddForce(startPoint.forward * force);
        NetworkServer.Spawn(airdropMarker);

        yield return new WaitForSeconds(startupDelay);

        for (int i = 0; i < missleCount; i++)
        {
            Vector3 dir = new Vector3(airdropMarker.transform.position.x, 30f, airdropMarker.transform.position.z);
            GameObject missle = Instantiate(server.smokeMissilePrefab, dir + new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6)), server.smokeMissilePrefab.transform.rotation);
            NetworkServer.Spawn(missle);
            yield return new WaitForSeconds(missleDelay);
        }
        Destroy(airdropMarker, 3f);
    }
}
