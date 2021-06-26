using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_LaunchStrategem : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.LAUNCH_STRATEGEM;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL          => true,
            FSMState_Character_Type.STEALTH_NORMAL  => false,
            FSMState_Character_Type.STEALTH_CLIMB   => false,
            _ => throw new System.NotImplementedException()
        };
    }
}