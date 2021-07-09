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
        public float normalSpeed;
        public float gravityMod;
        [Header("Stealth")]
        public float stealthSpeed;
        public float stealthClimbSpeed;
        public float stealthExitSpeed;
    }

    public class PlayerData
    {
        public NetworkCharacter networkCharacter;
        public CharacterController characterController;
        public Transform cameraTarget;

        public Vector3 velocity = new Vector3();
    }

    public enum FSMSTATE_CHARACTER_TYPE
    {
        NULL = 0,

        NORMAL,
        STEALTH_NORMAL,
        STEALTH_CLIMB,
        STRATEGEM
    }

    public interface IFSMState_Character_Base : IFSMState_Base
    {
        public FSMSTATE_CHARACTER_TYPE type { get; }

        public PlayerSettings playerSettings { get; set; }
        public PlayerData playerData { get; set; }
        public void OnEnter(FSMSTATE_CHARACTER_TYPE previousState);
        public void OnExit(FSMSTATE_CHARACTER_TYPE nextState);
        public FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist);
    }

    public abstract class FSMState_Character_Base : IFSMState_Character_Base
    {
        public abstract FSMSTATE_CHARACTER_TYPE type { get; }

        public PlayerSettings playerSettings { get; set; }
        public PlayerData playerData { get; set; }
        protected CharacterController playerController { get => playerData.characterController; }

        public virtual void OnEnter(FSMSTATE_CHARACTER_TYPE previousState) { }
        public virtual void OnExit(FSMSTATE_CHARACTER_TYPE nextState) { }

        public virtual FSMSTATE_CHARACTER_TYPE OnUpdate(Vector3 desiredMovementDir, float desiredMovementDist, out float remainingMovementDist)
        {
            remainingMovementDist = 0.0f;
            return FSMSTATE_CHARACTER_TYPE.NULL;
        }


        protected virtual void ApplyGravity()
        {
            Vector3 tempVel;
            playerData.velocity += Physics.gravity * Time.deltaTime * playerSettings.gravityMod;
            playerController.Move(playerData.velocity * Time.deltaTime);
            if (playerController.isGrounded)
            {
                playerData.velocity = new Vector3(playerData.velocity.x, 0.0f, playerData.velocity.z);
            }

            // End my life
            tempVel = playerData.velocity;
            playerData.velocity *= 0.9f;
            playerData.velocity.y = tempVel.y;
        }
    }
}