using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public abstract class FSMState_Character_StealthBase : FSMState_Character_Base
    {
        protected struct CharacterController_Settings
        {
            public Vector3 center;
            public float height;
            public float radius;
        }

        private CharacterController_Settings originalControllerSettings;

        public override void OnEnter()
        {
            originalControllerSettings = new CharacterController_Settings
            {
                center = playerController.center,
                height = playerController.height,
                radius = playerController.radius
            };

            playerController.height = 0.1f;
            playerController.radius = 0.1f;
            playerController.center = new Vector3(playerController.center.x, 0.05f, playerController.center.z);
        }

        public override void OnExit()
        {
            playerController.height = originalControllerSettings.height;
            playerController.radius = originalControllerSettings.radius;
            playerController.center = originalControllerSettings.center;
        }
    }
}
