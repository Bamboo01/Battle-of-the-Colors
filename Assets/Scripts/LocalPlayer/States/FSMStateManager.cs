using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor.Internal;

namespace CSZZGame.Refactor
{
    #region Public

    #region Generic

    /* 
     * Class: FSMStateManager_Generic
     * 
     * Description: Barebones state machine.
     *              Use when you only need to keep track of the current state
    */

    public interface IFSMStateManager_Generic<Key> : I_IFSMStateManager_Abstract<Key, IFSMState_Base>
    {
    }
    public class FSMStateManager_Generic<Key> : I_FSMStateManager_Basic<Key, IFSMState_Base>, IFSMStateManager_Generic<Key>
    {
    }

    #endregion

    #region UpdatableBasic

    /* 
     * Class: FSMStateManager_UpdatableBasic
     * 
     * Description: State machine with a basic update function
    */

    public interface IFSMStateManager_UpdatableBasic<Key> : I_IFSMStateManager_UpdatableBasic<Key, IFSMState_UpdatableBasic<Key>>
    {
    }
    public class FSMStateManager_UpdatableBasic<Key> : I_FSMStateManager_UpdatableBasic<Key, IFSMState_UpdatableBasic<Key>>, IFSMStateManager_UpdatableBasic<Key>
    {
    }

    #endregion

    #endregion

    #region Internal

    namespace Internal
    {
        #region Interfaces

        // Indicates that whatever inherits this interface is a state manager
        public interface I_IFSMStateManager_AbstractIdentifier
        {
        }

        // Defines a state manager that maps key values to objects
        public interface I_IFSMStateManager_Abstract<Key, State> : I_IFSMStateManager_AbstractIdentifier
        {
            public void Init(Key firstStateKey);
            public void AddState(Key stateKey, State newState);
            public T AddState<T>(Key stateKey) where T : State, new();
            public void ChangeState(Key stateKey);
        }

        // Defines a state manager that supports a basic update function
        public interface I_IFSMStateManager_UpdatableBasic<Key, State> : I_IFSMStateManager_Abstract<Key, State>
        {
            public void Update();
        }

        #endregion

        #region Classes

        // Holds all state objects added to a state manager
        public class FSMStateHolder<Key, State> where State : class
        {
            protected Dictionary<Key, State> stateDictionary = new Dictionary<Key, State>();
            public State currentState { get; protected set; } = null;

            public virtual void AddState(Key stateKey, State newState)
            {
                stateDictionary.Add(stateKey, newState);
            }
            public virtual void ChangeState(Key nextStateKey)
            {
                if (stateDictionary.TryGetValue(nextStateKey, out State nextState))
                {
                    currentState = nextState;
                }
                else
                {
                    Debug.LogErrorFormat("Unable to find state with key \"{0}\"", nextStateKey);
                }
            }
            public bool ContainsState(Key stateKey)
            {
                return stateDictionary.ContainsKey(stateKey);
            }
        }

        // Abstract state machine that only sets up state holder functions
        public abstract class I_FSMStateManager_Abstract<Key, State> : I_IFSMStateManager_Abstract<Key, State> where State : class
        {
            protected FSMStateHolder<Key, State> stateHolder = new FSMStateHolder<Key, State>();

            public virtual void Init(Key firstStateKey)
            {
                ChangeState(firstStateKey);
            }

            public virtual void AddState(Key stateKey, State newState)
            {
                stateHolder.AddState(stateKey, newState);
            }

            public virtual T AddState<T>(Key stateKey) where T : State, new()
            {
                T newState = new T();
                AddState(stateKey, newState);
                return newState;
            }

            public virtual void ChangeState(Key stateKey)
            {
                if (!stateHolder.ContainsState(stateKey))
                {
                    Debug.LogErrorFormat("State with key \"{0}\" does not exist.", stateKey);
                    return;
                }

                SwitchState(stateKey);
            }

            protected abstract void SwitchState(Key stateKey);
        }

        // Basic state machine for specialization into specific state classes
        public class I_FSMStateManager_Basic<Key, State> : I_FSMStateManager_Abstract<Key, State> where State : class, IFSMState_Base
        {
            protected override void SwitchState(Key stateKey)
            {
                Debug.Log("Switching states");
                if (stateHolder.currentState != null)
                {
                    stateHolder.currentState.OnExit();
                }
                stateHolder.ChangeState(stateKey);
                stateHolder.currentState.OnEnter();
            }
        }

        // State machine with basic update support
        public class I_FSMStateManager_UpdatableBasic<Key, State> : I_FSMStateManager_Basic<Key, State>, I_IFSMStateManager_UpdatableBasic<Key, State> where State : class, IFSMState_UpdatableBasic<Key>
        {
            public void Update()
            {
                Key nextStateKey = stateHolder.currentState.OnUpdate();

                if (nextStateKey != null)
                    ChangeState(nextStateKey);
            }
        }

        #endregion
    }

    #endregion
}