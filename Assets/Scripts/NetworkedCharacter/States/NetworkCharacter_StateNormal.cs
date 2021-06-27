using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_Normal : FSMState_Character_Base
    {
        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.NORMAL;

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist)
        {
            if (desiredMovementDist > 0.0f)
                playerController.Move(desiredMovementDir * desiredMovementDist * playerSettings.normalSpeed * Time.deltaTime);

            ApplyGravity();
            remainingMovementDist = -1.0f;
            return FSMSTATE_CHARACTER_TYPE.NULL;
        }
    }
}