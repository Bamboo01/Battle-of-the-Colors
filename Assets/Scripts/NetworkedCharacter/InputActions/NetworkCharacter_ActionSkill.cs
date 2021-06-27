using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_Skill : IAction_Character
    {
        public ACTION_CHARACTER_TYPE identifier => ACTION_CHARACTER_TYPE.SKILL;
        public bool IsActionAllowed(FSMSTATE_CHARACTER_TYPE stateType) => stateType switch
        {
            FSMSTATE_CHARACTER_TYPE.NORMAL          => true,
            FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL  => false,
            FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB   => false,
            _ => throw new System.NotImplementedException()
        };

        public int skillID { get; private set; }

        public Action_Character_Skill(int skillID)
        {
            this.skillID = skillID;
        }
    }
}