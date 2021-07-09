using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Refactor
{
    /// <summary>
    /// Defines a state manager that maps key values to state objects.
    /// </summary>
    /// <typeparam name="Key">The variable type used to reference states.</typeparam>
    public interface IFSMStateManager_Basic<Key>
    {
        public void Init(Key firstStateKey);
        public void AddState(Key stateKey, IFSMState_Base newState);
        public T AddState<T>(Key stateKey) where T : IFSMState_Base, new();
        public void ChangeState(Key stateKey);
    }

    /// <summary>
    /// Basic state machine that only handles changing of states.
    /// </summary>
    /// <typeparam name="Key">The variable type used to reference states.</typeparam>
    public class FSMStateManager_Basic<Key> : IFSMStateManager_Basic<Key>
    {
        protected Dictionary<Key, IFSMState_Base> stateDictionary = new Dictionary<Key, IFSMState_Base>();

        /// <summary>
        /// Get the currently active state object.
        /// <para>
        /// Replace this accessor with your preferred state class when extending this class.
        /// View NetworkCharacter_StateManager.cs for reference
        /// </para>
        /// </summary>
        protected IFSMState_Base currentState { get; set; } = null;

        protected Key currentStateKey { get; set; }

        public virtual void Init(Key firstStateKey)
        {
            ChangeState(firstStateKey);
        }

        public virtual void AddState(Key stateKey, IFSMState_Base newState)
        {
            if (!CheckStateType(newState))
                throw new System.ArgumentException("The added state is not compatible with this state machine!");

            SetupAddedState(newState);
            stateDictionary.Add(stateKey, newState);
        }
        // Check if the newly added state object is valid within this state machine
        protected virtual bool CheckStateType(IFSMState_Base state)
        {
            return true;
        }
        protected virtual void SetupAddedState(IFSMState_Base newState) { }

        public virtual T AddState<T>(Key stateKey) where T : IFSMState_Base, new()
        {
            T newState = new T();
            AddState(stateKey, newState);
            return newState;
        }

        public virtual void ChangeState(Key stateKey)
        {
            IFSMState_Base nextState;

            try
            {
                nextState = stateDictionary[stateKey];
            }
            catch (KeyNotFoundException)
            {
                Debug.LogErrorFormat("State with key \"{0}\" does not exist.", stateKey);
                return;
            }

            if (currentState != null)
            {
                // If the new state is the same as the current state, return
                if (EqualityComparer<Key>.Default.Equals(currentStateKey, stateKey))
                    return;

                OnStateExit(currentState, nextState);
            }

            Debug.Log("Switching states");
            IFSMState_Base previousState = currentState;
            currentState = stateDictionary[stateKey];
            currentStateKey = stateKey;

            OnStateEnter(previousState, currentState);
        }
        protected virtual void OnStateExit(IFSMState_Base currentState, IFSMState_Base nextState) { }
        protected virtual void OnStateEnter(IFSMState_Base previousState, IFSMState_Base currentState) { }
    }
}