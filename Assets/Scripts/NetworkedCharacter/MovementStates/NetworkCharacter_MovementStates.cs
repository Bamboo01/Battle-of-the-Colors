using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame;
using CSZZGame.Refactor;

namespace CSZZGame.Character.Movement
{
    [System.Serializable]
    public struct PlayerSettings_Movement
    {
        [HideInInspector]
        public CharacterController characterController;

        public float normalSpeed;

        [Header("Stealth")]
        public float stealthSpeed;
    }

    public interface IFSMState_Movement_Base : IFSMState_Base
    {
        public PlayerSettings_Movement playerSettings { get; set; }
        public string OnUpdate(Vector3 desiredMovement);
    }

    public interface IFSMStateManager_Movement : Refactor.Internal.I_IFSMStateManager_Abstract<string, IFSMState_Movement_Base>
    {
        public void SetPlayerSettings(PlayerSettings_Movement playerSettings);
        public void Update(Vector3 desiredMovement);
    }

    public class FSMStateManager_Movement : Refactor.Internal.I_FSMStateManager_Basic<string, IFSMState_Movement_Base>, IFSMStateManager_Movement
    {
        private PlayerSettings_Movement playerSettings;

        public void SetPlayerSettings(PlayerSettings_Movement playerSettings)
        {
            this.playerSettings = playerSettings;
        }

        public override void AddState(string stateKey, IFSMState_Movement_Base newState)
        {
            newState.playerSettings = playerSettings;
            base.AddState(stateKey, newState);
        }

        public void Update(Vector3 desiredMovement)
        {
            stateHolder.currentState.OnUpdate(desiredMovement);
        }
    }

    public abstract class FSMState_Movement_Base : IFSMState_Movement_Base
    {
        public PlayerSettings_Movement playerSettings { get; set; }
        protected CharacterController playerController { get => playerSettings.characterController; }

        public virtual void OnEnter()
        {
        }

        public virtual string OnUpdate(Vector3 desiredMovement)
        {
            return null;
        }

        public virtual void OnExit()
        {
        }
    }

    public class FSMState_Movement_Normal : FSMState_Movement_Base
    {
        public override string OnUpdate(Vector3 desiredMovement)
        {
            playerController.Move((desiredMovement * playerSettings.normalSpeed + Physics.gravity) * Time.deltaTime);

            // TODO: Check for state switch conditions
            return null;
        }
    }

    public abstract class FSMState_Movement_StealthBase : FSMState_Movement_Base
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

            playerController.Move(new Vector3(
                0.0f, 
                -(originalControllerSettings.center.y - playerController.center.y),
                0.0f
            ));
        }
    }

    public class FSMState_Movement_StealthNormal : FSMState_Movement_Base
    {

    }
}
