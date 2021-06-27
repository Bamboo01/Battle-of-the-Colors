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
        public NetworkCharacter networkCharacter;
        [HideInInspector]
        public CharacterController characterController;
        [HideInInspector]
        public Transform cameraTarget;

        public float normalSpeed;

        [Header("Stealth")]
        public float stealthSpeed;
    }

    public enum FSMSTATE_CHARACTER_TYPE
    {
        NULL = 0,

        NORMAL,
        STEALTH_NORMAL,
        STEALTH_CLIMB
    }

    public interface IFSMState_Character_Base : IFSMState_Base
    {
        public FSMSTATE_CHARACTER_TYPE type { get; }

        public PlayerSettings playerSettings { get; set; }
        public FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovement);
    }

    public abstract class FSMState_Character_Base : IFSMState_Character_Base
    {
        public abstract FSMSTATE_CHARACTER_TYPE type { get; }

        public PlayerSettings playerSettings { get; set; }
        protected CharacterController playerController { get => playerSettings.characterController; }

        public virtual void OnEnter()
        {
        }

        public virtual FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovement)
        {
            return FSMSTATE_CHARACTER_TYPE.NULL;
        }

        public virtual void OnExit()
        {
        }
    }
}
