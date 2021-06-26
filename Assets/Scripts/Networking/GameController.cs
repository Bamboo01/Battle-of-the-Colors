using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    [SerializeField] double MatchDuration = 300;
    double initialGameTime = 0;
    double currentGameTime = 0;

    [SerializeField] TextMeshProUGUI timerText;


    public override void OnStartClient()
    {
        base.OnStartClient();
        InitializeServer();
    }

    void InitializeServer()
    {
        initialGameTime = NetworkTime.time;
    }

    private void Update()
    {
        if(NetworkTime.time < initialGameTime + MatchDuration)
        {
            currentGameTime = MatchDuration - (NetworkTime.time - initialGameTime);
            TimeSpan t = TimeSpan.FromSeconds(currentGameTime);
            string timeString = "";
            if (t.Minutes > 0)
            {
                timeString = t.ToString(@"mm\:ss");
            }
            else
            {
                timeString = t.ToString(@"ss\:ff");
            }
            timerText.text = timeString;
        }
    }


}
