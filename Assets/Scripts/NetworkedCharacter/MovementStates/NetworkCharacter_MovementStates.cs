using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame;
using CSZZGame.Refactor;

namespace CSZZGame.Character.Movement
{
    public interface IFSMState_Movement_Base : IFSMState_Base
    {
        public CharacterController playerCharacter { get; set; }
        public float playerSpeed { get; set; }
        public string OnUpdate(Vector3 desiredMovement);
    }

    public interface IFSMStateManager_Movement : Refactor.Internal.I_IFSMStateManager_Abstract<string, IFSMState_Movement_Base>
    {
        public void SetPlayerSettings(CharacterController characterController, float speed);
        public void Update(Vector3 desiredMovement);
    }

    public class FSMStateManager_Movement : Refactor.Internal.I_FSMStateManager_Basic<string, IFSMState_Movement_Base>, IFSMStateManager_Movement
    {
        private CharacterController playerCharacter = null;
        private float playerSpeed = 1.0f;

        public void SetPlayerSettings(CharacterController characterController, float speed)
        {
            playerCharacter = characterController;
            playerSpeed = speed;
        }

        public override void AddState(string stateKey, IFSMState_Movement_Base newState)
        {
            newState.playerCharacter = playerCharacter;
            newState.playerSpeed = playerSpeed;
            base.AddState(stateKey, newState);
        }

        public void Update(Vector3 desiredMovement)
        {
            stateHolder.currentState.OnUpdate(desiredMovement);
        }
    }

    public abstract class FSMState_Movement_Base : IFSMState_Movement_Base
    {
        public CharacterController playerCharacter { get; set; } = null;
        public float playerSpeed { get; set; } = 1.0f;

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
            playerCharacter.Move((desiredMovement * playerSpeed + Physics.gravity) * Time.deltaTime);

            // TODO: Check for state switch conditions
            return null;
        }
    }
}
