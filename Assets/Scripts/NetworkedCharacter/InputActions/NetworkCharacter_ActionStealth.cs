using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_Stealth : IAction_Character
    {
        public ACTION_CHARACTER_TYPE identifier => ACTION_CHARACTER_TYPE.STEALTH;
        public bool IsActionAllowed(FSMSTATE_CHARACTER_TYPE stateType) => stateType switch
        {
            FSMSTATE_CHARACTER_TYPE.NORMAL          => startStealth,
            FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL  => !startStealth,
            FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB   => !startStealth,
            FSMSTATE_CHARACTER_TYPE.STRATEGEM       => false,
            _ => throw new System.NotImplementedException()
        };

        public bool startStealth { get; private set; }

        public Action_Character_Stealth(bool isStart)
        {
            startStealth = isStart;
        }
    }
}