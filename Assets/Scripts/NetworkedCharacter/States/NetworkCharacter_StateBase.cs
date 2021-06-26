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
}
