using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Refactor
{
    public class EventChannels
    {
        static public string OnInputEvent = "OnInputEventChannel";
        static public string OnOwnCharacterActionEvent = "OnOwnCharacterActionEventChannel";
        static public string OnOwnCharacterStateChangeEvent = "OnOwnCharacterStateChangeEventChannel";
        static public string OnPaintPaintableEvent = "OnPaintPaintableChannel";
        static public string OnClientPlayerDeath = "OnClientPlayerDeathChannel";
        static public string OnServerPlayerDeath = "OnServerPlayerDeathChannel";
        static public string OnServerPlayerRespawn = "OnPlayerRespawnChannel";
    }
}
