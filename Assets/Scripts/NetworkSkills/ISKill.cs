using CSZZGame.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    void UseSkill(Transform startPoint, ServerCharacterData data, CSZZServerHandler serverHandler, NetworkRoomManagerScript server);
}
