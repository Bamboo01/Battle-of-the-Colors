using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public enum ACTION_CHARACTER_TYPE
    {
        NO_MOVE,
        MOVE,
        STEALTH,
        SHOOT,
        TOGGLE_STRATEGEM_MODE,
        LAUNCH_STRATEGEM
    }

    public interface IAction_Character
    {
        public ACTION_CHARACTER_TYPE identifier { get; }

        public bool IsActionAllowed(FSMSTATE_CHARACTER_TYPE stateType);
    }
}