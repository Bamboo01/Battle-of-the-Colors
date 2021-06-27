using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_Move : IAction_Character
    {
        public ACTION_CHARACTER_TYPE identifier => ACTION_CHARACTER_TYPE.MOVE;
        public bool IsActionAllowed(FSMSTATE_CHARACTER_TYPE stateType) => stateType switch
        {
            FSMSTATE_CHARACTER_TYPE.NORMAL          => true,
            FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL  => true,
            FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB   => true,
            _ => throw new System.NotImplementedException()
        };

        public Vector3 desiredMovement { get; private set; }

        public Action_Character_Move(Vector3 desiredMovement)
        {
            this.desiredMovement = desiredMovement;
        }
    }
}