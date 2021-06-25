using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;

namespace CSZZGame.Character.Movement
{
    // Modify the signature of Update() from IFSMStateBase
    public interface IFSMState_Movement_Base : IFSMState_AbstractIdentifier
    {
        public void OnUpdate(Vector3 desiredMovement);

        #region Forwarded Functions
        public void OnEnter();
        public void OnExit();
        #endregion
    }

    // Modify the signature of Update() from IFSMStateManager_Abstract<T, U>
    public interface IFSMStateManager_Movement<T> : IFSMStateManager_AbstractIdentifier
    {
        public void Update(Vector3 desiredMovement);

        #region Forwarded Functions
        public void Init(CharacterController playerCharacter, T firstStateKey);
        public void ChangeState(T stateKey);
        public void AddState(T stateKey, IFSMState_Movement_Base newState);
        public U AddState<U>(T stateKey) where U : IFSMState_Movement_Base, new();
        #endregion
    }

    public class Character_Movement_StateManagerBase : IFSMStateManager_Movement<string>
    {
        protected FSMStateHolder<string, IFSMState_Movement_Base> stateHolder = new FSMStateHolder<string, IFSMState_Movement_Base>();
        private CharacterController playerCharacter = null;

        public void Init(CharacterController playerCharacter, string firstStateKey)
        {
            this.playerCharacter = playerCharacter;
            ChangeState(firstStateKey);
        }

        public void AddState(string stateKey, IFSMState_Movement_Base newState)
        {
            stateHolder.AddState(stateKey, newState);
        }

        public T AddState<T>(string stateKey) where T : IFSMState_Movement_Base, new()
        {
            T newState = new T();
            stateHolder.AddState(stateKey, newState);
            return newState;
        }

        public void ChangeState(string stateKey)
        {
            stateHolder.ChangeState(stateKey);
        }

        public virtual void Update(Vector3 desiredMovement)
        {

        }
    }

    public abstract class Character_Movement_StateBase : IFSMState_Movement_Base
    {
        public virtual void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnUpdate(Vector3 desiredMovement)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Character_Movement_StateNormal : Character_Movement_StateBase
    {
        public override void OnEnter()
        {
        }

        public override void OnUpdate(Vector3 desiredMovement)
        {
            base.OnUpdate(desiredMovement);
        }

        public override void OnExit()
        {
        }
    }
}
