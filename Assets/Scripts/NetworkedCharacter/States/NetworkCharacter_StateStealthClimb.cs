using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_StealthClimb : FSMState_Character_StealthBase
    {
        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB;

        public override void OnEnter(FSMSTATE_CHARACTER_TYPE previousState)
        {
            jumpOffDir = Vector3.zero;
            base.OnEnter(previousState);
        }

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist)
        {
            if (desiredMovementDist <= 0.0f)
            {
                remainingMovementDist = -1.0f;
                return FSMSTATE_CHARACTER_TYPE.NULL;
            }

            Vector3 surfaceNormal;
            var stealthCheckFlags = CheckValidStealthPosition(-desiredMovementDir, out surfaceNormal);

            // Move only if we are on valid paint against the wall
            if ((stealthCheckFlags & STEALTH_RESULTFLAG.VALID) != 0)
            {
                remainingMovementDist = -1.0f;

                // Get the direction to move in relative to the surface based on the player's inputted desired movement direction
                /// This was so hard to visualize lol
                Vector3 worldUp = new Vector3(0.0f, 1.0f, 0.0f);
                Vector3 surfaceRightVector = Vector3.Cross(worldUp, surfaceNormal).normalized;
                float angleToRotate = Vector3.Angle(worldUp, surfaceNormal);
                Quaternion rotation = Quaternion.AngleAxis(angleToRotate, surfaceRightVector);
                Vector3 resultantMovementDir = rotation * desiredMovementDir;

                jumpOffDir = surfaceNormal;

                playerController.Move(resultantMovementDir * desiredMovementDist * playerSettings.stealthClimbSpeed * Time.deltaTime);
                playerData.velocity = new Vector3(playerData.velocity.x, 0.0f, playerData.velocity.z);

                // Test if we are no longer against a wall
                playerController.Move(-surfaceNormal * 0.1f);
                if ((playerController.collisionFlags & CollisionFlags.CollidedSides) == 0)
                    return FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL;
                else
                    return FSMSTATE_CHARACTER_TYPE.NULL;
            }
            else // We have collided with an unclimbable area of the wall
            {
                //ApplyGravity();
                jumpOffDir = Vector3.zero;
                remainingMovementDist = desiredMovementDist;
                return FSMSTATE_CHARACTER_TYPE.NORMAL;
            }
        }
    }
}