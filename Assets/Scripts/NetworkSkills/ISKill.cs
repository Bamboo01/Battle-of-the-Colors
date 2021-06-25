using CSZZGame.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    void UseSkill(Transform startPoint, NetworkRoomManagerScript server);
}
