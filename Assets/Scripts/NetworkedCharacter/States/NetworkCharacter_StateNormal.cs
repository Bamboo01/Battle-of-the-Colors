using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public class FSMState_Character_Normal : FSMState_Character_Base
    {
        public override FSMSTATE_CHARACTER_TYPE type => FSMSTATE_CHARACTER_TYPE.NORMAL;

        public override FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovement)
        {
            playerController.Move((desiredMovement * playerSettings.normalSpeed + Physics.gravity) * Time.deltaTime);

            return FSMSTATE_CHARACTER_TYPE.NULL;
        }
    }
}