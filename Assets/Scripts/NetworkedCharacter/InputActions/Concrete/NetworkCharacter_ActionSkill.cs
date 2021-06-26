using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_Skill : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.SKILL;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL          => true,
            FSMState_Character_Type.STEALTH_NORMAL  => false,
            FSMState_Character_Type.STEALTH_CLIMB   => false,
            _ => throw new System.NotImplementedException()
        };

        public int skillID { get; private set; }

        public Action_Character_Skill(int skillID)
        {
            this.skillID = skillID;
        }
    }
}