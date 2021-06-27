using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_StealthNormal : FSMState_Character_StealthBase
    {
        public override FSMState_Character_Type type => FSMState_Character_Type.STEALTH_NORMAL;

        public override FSMState_Character_Type OnUpdate(Vector3 desiredMovement)
        {
            playerController.Move((desiredMovement * playerSettings.stealthSpeed + Physics.gravity) * Time.deltaTime);

            return FSMState_Character_Type.NULL;
        }
    }
}