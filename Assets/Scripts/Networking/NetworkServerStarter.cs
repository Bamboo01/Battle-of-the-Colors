using UnityEngine;
using System;
using System.Linq;
using Mirror;

public class NetworkServerStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var args = Environment.GetCommandLineArgs();
        if (args.Any((arg) => arg == "servermode"))
        {
            Application.targetFrameRate = 60;
            Time.fixedDeltaTime = 1 / 60f;
            NetworkRoomManager.singleton.StartServer();
        }
    }

    [ContextMenu("Force Start Server")]
    public void startServer()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1 / 60f;
        NetworkRoomManager.singleton.StartServer();
    }
}
