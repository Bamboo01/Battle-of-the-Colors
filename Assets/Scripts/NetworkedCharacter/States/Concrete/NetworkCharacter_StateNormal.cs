using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_Normal : FSMState_Character_Base
    {
        public override FSMState_Character_Type type => FSMState_Character_Type.NORMAL;

        public override FSMState_Character_Type OnUpdate(Vector3 desiredMovement)
        {
            playerController.Move((desiredMovement * playerSettings.normalSpeed + Physics.gravity) * Time.deltaTime);

            return FSMState_Character_Type.NULL;
        }
    }
}