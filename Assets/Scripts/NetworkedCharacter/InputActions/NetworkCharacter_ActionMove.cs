using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class Action_Character_Move : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.MOVE;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL          => true,
            FSMState_Character_Type.STEALTH_NORMAL  => true,
            FSMState_Character_Type.STEALTH_CLIMB   => true,
            _ => throw new System.NotImplementedException()
        };

        public Vector3 desiredMovement { get; private set; }

        public Action_Character_Move(Vector3 desiredMovement)
        {
            this.desiredMovement = desiredMovement;
        }
    }
}