using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_Stealth : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.STEALTH;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL          => startStealth,
            FSMState_Character_Type.STEALTH_NORMAL  => !startStealth,
            FSMState_Character_Type.STEALTH_CLIMB   => !startStealth,
            _ => throw new System.NotImplementedException()
        };

        public bool startStealth { get; private set; }

        public Action_Character_Stealth(bool isStart)
        {
            startStealth = isStart;
        }
    }
}