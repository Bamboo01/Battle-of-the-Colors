using CSZZGame.Networking;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMissle : MonoBehaviour, ISkill
{
    
    float missleCount = 6;
    float startupDelay = 0.5f;
    float missleDelay = 0.1f;
    [SerializeField] float force = 30f;

    public void UseSkill(Transform startPoint, NetworkRoomManagerScript server)
    {   
        StartCoroutine(SpawnMissles(startPoint, server));
    }

    IEnumerator SpawnMissles(Transform startPoint, NetworkRoomManagerScript server)
    {
        GameObject airdropMarker = Instantiate(server.markerPrefab, startPoint.position, Quaternion.identity);
        airdropMarker.GetComponent<Rigidbody>().AddForce(startPoint.forward * force);
        NetworkServer.Spawn(airdropMarker);
        yield return new WaitForSeconds(startupDelay);

        for (int i = 0; i < missleCount; i++)
        {
            Vector3 dir = new Vector3(airdropMarker.transform.position.x, 30f, airdropMarker.transform.position.z);
            GameObject missle = Instantiate(server.smokePrefab, dir + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)), Quaternion.identity);
            NetworkServer.Spawn(missle);
            yield return new WaitForSeconds(missleDelay);
        }
    }
}
