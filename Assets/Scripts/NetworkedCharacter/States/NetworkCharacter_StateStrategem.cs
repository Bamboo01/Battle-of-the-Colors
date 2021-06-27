using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;

namespace CSZZGame.Character
{
    public class FSMState_Character_Strategem : FSMState_Character_Base
    {
        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.STRATEGEM;

        public override void OnEnter()
        {
            StrategemManager.Instance.strategemActivation();
        }
        public override void OnExit()
        {
            StrategemManager.Instance.strategemDeactivation();
        }

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist)
        {
            ApplyGravity();
            remainingMovementDist = -1.0f;
            return FSMSTATE_CHARACTER_TYPE.NULL;
        }
    }
}