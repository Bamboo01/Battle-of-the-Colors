using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CSZZGame.Networking;

public class NetworkServer : MonoBehaviour
{
    List<NetworkConnectionToClient> clientConnections = new List<NetworkConnectionToClient>();
    NetworkRoomManagerScript networkManager;
    void Awake()
    {
        networkManager = (NetworkRoomManagerScript)NetworkManager.singleton;
    }

    public void spawnCharacter(NetworkConnectionToClient sender)
    {
        if (clientConnections.Contains(sender))
        {
            return;
        }
        clientConnections.Add(sender);
        GameObject playerCharacter = Instantiate(networkManager.playerCharacterPrefab);
        Mirror.NetworkServer.Spawn(playerCharacter, sender);
    }
}
