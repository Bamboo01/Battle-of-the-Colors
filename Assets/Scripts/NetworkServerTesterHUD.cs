using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Events;
using CSZZGame.Networking;

public class NetworkServerTesterHUD : MonoBehaviour
{
    string eventTestString = "";
    string sentString = "";
    int messageCounter = 0;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 410f, Screen.height - 30f, 400.0f, 30f));
            if (GUILayout.Button("Send Event"))
            {
                CSZZNetworkInterface.Instance.SendNetworkEvent("TestChannel", new string[2] { NetworkClient.connection.ToString() + ":", sentString });
                sentString = "";
            }    
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width - 410f, (Screen.height - 50f), 400.0f, 30));
            sentString = GUILayout.TextField(sentString, 100);
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width - 410f, (Screen.height - 70f) - (15.0f * (float)messageCounter), 400.0f, Screen.height - 100.0f));
            GUILayout.TextArea(eventTestString);
        GUILayout.EndArea();
    }

    void Start()
    {
        EventManager.Instance.Listen("TestChannel", TestResponse);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void TestResponse(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<byte[]> info = (EventRequestInfo<byte[]>)eventRequestInfo;
        string[] message = CSZZNetworkInterface.DeserializeEventData<string[]>(info.body);
        for (int i = 0; i < message.Length; i++)
        {
            if (messageCounter > 10)
            {
                eventTestString = "";
                messageCounter = 0;
            }
            messageCounter++;
            eventTestString += message[i] + "\n";
        }
    }
}
