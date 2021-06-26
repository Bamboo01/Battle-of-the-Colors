using CSZZGame.Networking;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2Shield : MonoBehaviour, ISkill
{
    // The ACTUAL color of the particle
    private Color color;
    public void ServerSetup(Color c)
    {
        color = c;
    }


    public void UseSkill(Transform startPoint, ServerCharacterData data, NetworkRoomManagerScript server)
    {
        GameObject shield = Instantiate(server.shieldPrefab);
        shield.transform.position = transform.position;

        //Code to align shield to floor and slope, while facing char dir. I can't get it to work on a downwards facing slope though
        Physics.Raycast(shield.transform.position + Vector3.up * 3f, Vector3.down, out RaycastHit hit, 10f);
        shield.transform.position = hit.point;
        shield.transform.up = hit.normal;
        var localPos = shield.transform.InverseTransformDirection((transform.position + transform.forward) - shield.transform.position);
        localPos.y = 0;
        Vector3 lookPos = shield.transform.position + shield.transform.TransformDirection(localPos);
        shield.transform.LookAt(lookPos, shield.transform.up);

        ServerSetup(ServerCharacterData.teamToColor(data.characterTeam));
        NetworkServer.Spawn(shield);
    }
}
