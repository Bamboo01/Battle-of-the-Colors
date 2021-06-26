using CSZZGame.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StrategemBase : MonoBehaviour
{
    public abstract void UseSkill(Transform startPoint, ServerCharacterData data, CSZZServerHandler serverHandler, NetworkRoomManagerScript server, NetworkCharacter caller);
}
