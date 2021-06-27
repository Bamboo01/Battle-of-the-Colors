using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_StealthNormal : FSMState_Character_StealthBase
    {
        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL;

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovement)
        {
            if (CheckValidStealthPosition(cameraTarget.up))
                playerController.Move(desiredMovement * playerSettings.stealthSpeed * Time.deltaTime);


            playerController.Move(Physics.gravity * Time.deltaTime);
            return FSMSTATE_CHARACTER_TYPE.NULL;
        }
    }
}