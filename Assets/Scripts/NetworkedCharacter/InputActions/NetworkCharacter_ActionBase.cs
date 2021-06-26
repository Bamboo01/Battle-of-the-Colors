using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public enum Action_Character_Type
    {
        NO_MOVE,
        MOVE,
        STEALTH,
        SHOOT,
        SKILL,
        LAUNCH_STRATEGEM
    }

    public interface IAction_Character
    {
        public Action_Character_Type identifier { get; }

        public bool IsActionAllowed(FSMState_Character_Type stateType);
    }
}