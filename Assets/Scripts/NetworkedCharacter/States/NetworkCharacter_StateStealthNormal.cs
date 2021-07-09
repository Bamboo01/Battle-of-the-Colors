using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_StealthNormal : FSMState_Character_StealthBase
    {
        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL;

        public override void OnEnter(FSMSTATE_CHARACTER_TYPE previousState)
        {
            jumpOffDir = Vector3.zero;
            base.OnEnter(previousState);
        }

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist)
        {
            Vector3 originalPosition = playerController.transform.position;

            /// Can't detect if we're next to wall for some reason
            /// Maybe the collision flags that we are seeing is only the result of the last Move() call from the previous frame
            // In case we are currently in the air, test if we are currently trying to push against a wall
            //playerController.Move(desiredMovementDir * 0.1f);
            //if ((playerController.collisionFlags & CollisionFlags.CollidedSides) != 0)
            //{
            //    Debug.Log("climb");
            //    remainingMovementDist = desiredMovementDist;
            //    return FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB;
            //}
            //else
            //    playerController.Move(desiredMovementDir * -0.1f);


            var stealthCheckFlags = CheckValidStealthPosition(cameraTarget.up, out _, 4.3f);

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
                return FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB;
            }

            jumpOffDir = Vector3.up;

            ApplyGravity();
            remainingMovementDist = -1.0f;
            return FSMSTATE_CHARACTER_TYPE.NULL;
        }
    }
}