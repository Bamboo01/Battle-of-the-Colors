using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_StealthClimb : FSMState_Character_StealthBase
    {
        private Vector3 lastNonZeroMovementDir = new Vector3();

        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB;

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist)
        {
            remainingMovementDist = -1.0f;
            if (desiredMovementDist <= 0.0f)
                return FSMSTATE_CHARACTER_TYPE.NULL;

            Vector3 surfaceNormal;
            var stealthCheckFlags = CheckValidStealthPosition(-desiredMovementDir, out surfaceNormal);

            // Move only if we are on valid paint against the wall
            if ((stealthCheckFlags & STEALTH_RESULTFLAG.VALID) != 0)
            {
                // Get the direction to move in relative to the surface based on the player's inputted desired movement direction
                /// This was so hard to visualize lol
                Vector3 worldUp = new Vector3(0.0f, 1.0f, 0.0f);
                Vector3 surfaceRightVector = Vector3.Cross(worldUp, surfaceNormal).normalized;
                float angleToRotate = Vector3.Angle(worldUp, surfaceNormal);
                Quaternion rotation = Quaternion.AngleAxis(angleToRotate, surfaceRightVector);
                Vector3 resultantMovementDir = rotation * desiredMovementDir;

                playerController.Move(resultantMovementDir * desiredMovementDist * playerSettings.stealthClimbSpeed * Time.deltaTime);

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
                return FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL;
            }
        }
    }
}