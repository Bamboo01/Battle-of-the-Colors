using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_Shoot : IAction_Character
    {
        public ACTION_CHARACTER_TYPE identifier => ACTION_CHARACTER_TYPE.SHOOT;
        public bool IsActionAllowed(FSMSTATE_CHARACTER_TYPE stateType) => stateType switch
        {
            FSMSTATE_CHARACTER_TYPE.NORMAL          => true,
            FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL  => false,
            FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB   => false,
            _ => throw new System.NotImplementedException()
        };
    }
}