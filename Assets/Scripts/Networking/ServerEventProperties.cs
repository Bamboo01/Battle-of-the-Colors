using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ServerEventProperties", menuName = "Server only events properties")]
public class ServerEventProperties : ScriptableObject
{
    public List<string> ServerOnlyChannels;
}
