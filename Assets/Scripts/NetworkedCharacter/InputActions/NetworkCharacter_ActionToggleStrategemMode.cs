using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_ToggleStrategemMode : IAction_Character
    {
        public ACTION_CHARACTER_TYPE identifier => ACTION_CHARACTER_TYPE.TOGGLE_STRATEGEM_MODE;
        public bool IsActionAllowed(FSMSTATE_CHARACTER_TYPE stateType) => stateType switch
        {
            FSMSTATE_CHARACTER_TYPE.NORMAL          => true,
            FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL  => true,
            FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB   => true,
            FSMSTATE_CHARACTER_TYPE.STRATEGEM       => true,
            _ => throw new System.NotImplementedException()
        };
    }
}