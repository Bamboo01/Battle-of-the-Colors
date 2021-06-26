using CSZZGame.Networking;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintMissile : StratagemBase
{
    float missleCount = 6;
    float startupDelay = 0.5f;
    float missleDelay = 0.1f;
    [SerializeField] float force = 30f;

    public override void UseSkill(Transform startPoint, ServerCharacterData data, CSZZServerHandler serverHandler, NetworkRoomManagerScript server, NetworkCharacter caller)
    {
        StartCoroutine(SpawnMissiles(startPoint, data, serverHandler, server));
    }

    IEnumerator SpawnMissiles(Transform startPoint, ServerCharacterData data, CSZZServerHandler serverHandler, NetworkRoomManagerScript server)
    {
        GameObject airdropMarker = Instantiate(server.markerPrefab, startPoint.position, server.markerPrefab.transform.rotation);
        airdropMarker.GetComponent<Rigidbody>().AddForce(startPoint.forward * force);
        NetworkServer.Spawn(airdropMarker);

        yield return new WaitForSeconds(startupDelay);

        for (int i = 0; i < missleCount; i++)
        {
            Vector3 dir = new Vector3(airdropMarker.transform.position.x, 30f, airdropMarker.transform.position.z);
            GameObject missle = Instantiate(server.paintMissilePrefab, dir + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)), server.paintMissilePrefab.transform.rotation);
            PaintMissileBehaviour missleBehaviour = missle.GetComponent<PaintMissileBehaviour>();
            missleBehaviour.server = serverHandler;
            missleBehaviour.serverCharacterData = data;

            NetworkServer.Spawn(missle);
            yield return new WaitForSeconds(missleDelay);
        }
        Destroy(airdropMarker, 3f);
    }
}
