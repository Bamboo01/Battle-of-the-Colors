using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_StealthNormal : FSMState_Character_StealthBase
    {
        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL;

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist)
        {

            Vector3 originalPosition = playerController.transform.position;
            var stealthCheckFlags = CheckValidStealthPosition(cameraTarget.up, out _);

            // Move only if we are on valid paint, or in the air
            if ((stealthCheckFlags & STEALTH_RESULTFLAG.VALID) != 0)
                playerController.Move(desiredMovementDir * desiredMovementDist * playerSettings.stealthSpeed * Time.deltaTime);

            // Start climbing if we collide with a wall
            if ((playerController.collisionFlags & CollisionFlags.Sides) != 0)
            {
                remainingMovementDist = desiredMovementDist - (playerController.transform.position - originalPosition).magnitude;
                return FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB;
            }

            ApplyGravity();
            remainingMovementDist = 0.0f;
            return FSMSTATE_CHARACTER_TYPE.NULL;
        }
    }
}