using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_StealthNormal : FSMState_Character_StealthBase
    {
        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL;

        public override void OnEnter()
        {
            jumpOffDir = Vector3.zero;
            base.OnEnter();
        }

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist)
        {
            Vector3 originalPosition = playerController.transform.position;
            var stealthCheckFlags = CheckValidStealthPosition(cameraTarget.up, out _);

            // Move only if we are on a valid paint surface
            if ((stealthCheckFlags & STEALTH_RESULTFLAG.VALID) != 0 && (stealthCheckFlags & STEALTH_RESULTFLAG.NO_HIT) == 0)
                playerController.Move(desiredMovementDir * desiredMovementDist * playerSettings.stealthSpeed * Time.deltaTime);
            else
            {
                remainingMovementDist = desiredMovementDist;
                return FSMSTATE_CHARACTER_TYPE.NORMAL;
            }

            // Start climbing if we collide with a wall
            if ((playerController.collisionFlags & CollisionFlags.CollidedSides) != 0)
            {
                remainingMovementDist = desiredMovementDist - (playerController.transform.position - originalPosition).magnitude;
                isEnteringAnotherStealth = true;
                return FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB;
            }

            jumpOffDir = Vector3.up;

            ApplyGravity();
            remainingMovementDist = -1.0f;
            return FSMSTATE_CHARACTER_TYPE.NULL;
        }
    }
}