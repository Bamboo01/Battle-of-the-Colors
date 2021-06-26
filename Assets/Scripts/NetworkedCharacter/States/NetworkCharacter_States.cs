using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame;
using CSZZGame.Refactor;

namespace CSZZGame.Character
{
    [System.Serializable]
    public struct PlayerSettings
    {
        [HideInInspector]
        public CharacterController characterController;

        public float normalSpeed;

        [Header("Stealth")]
        public float stealthSpeed;
    }

    public enum FSMState_Character_Type
    {
        NULL = 0,

        NORMAL,
        STEALTH_NORMAL,
        STEALTH_CLIMB
    }

    public interface IFSMState_Character_Base : IFSMState_Base
    {
        public FSMState_Character_Type type { get; }

        public PlayerSettings playerSettings { get; set; }
        public FSMState_Character_Type OnUpdate(Vector3 desiredMovement);
    }

    public interface IFSMStateManager_Character : Refactor.Internal.I_IFSMStateManager_Basic<FSMState_Character_Type, IFSMState_Character_Base>
    {
        public FSMState_Character_Type currentStateType { get; }

        public void SetPlayerSettings(PlayerSettings playerSettings);
        public void Update(Vector3 desiredMovement);
    }

    public class FSMStateManager_Character : Refactor.Internal.I_FSMStateManager_Basic<FSMState_Character_Type, IFSMState_Character_Base>, IFSMStateManager_Character
    {
        private PlayerSettings playerSettings;

        // Return the current state's type if it is valid, otherwise NULL
        public FSMState_Character_Type currentStateType => stateHolder.currentState?.type ?? FSMState_Character_Type.NULL;

        public void SetPlayerSettings(PlayerSettings playerSettings)
        {
            this.playerSettings = playerSettings;
        }

        public override void AddState(FSMState_Character_Type stateKey, IFSMState_Character_Base newState)
        {
            newState.playerSettings = playerSettings;
            base.AddState(stateKey, newState);
        }

        public void Update(Vector3 desiredMovement)
        {
            stateHolder.currentState.OnUpdate(desiredMovement);
        }
    }

    public abstract class FSMState_Character_Base : IFSMState_Character_Base
    {
        public abstract FSMState_Character_Type type { get; }

        public PlayerSettings playerSettings { get; set; }
        protected CharacterController playerController { get => playerSettings.characterController; }

        public virtual void OnEnter()
        {
        }

        public virtual FSMState_Character_Type OnUpdate(Vector3 desiredMovement)
        {
            return FSMState_Character_Type.NULL;
        }

        public virtual void OnExit()
        {
        }
    }

    public class FSMState_Character_Normal : FSMState_Character_Base
    {
        public override FSMState_Character_Type type => FSMState_Character_Type.NORMAL;

        public override FSMState_Character_Type OnUpdate(Vector3 desiredMovement)
        {
            playerController.Move((desiredMovement * playerSettings.normalSpeed + Physics.gravity) * Time.deltaTime);

            return FSMState_Character_Type.NULL;
        }
    }

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
