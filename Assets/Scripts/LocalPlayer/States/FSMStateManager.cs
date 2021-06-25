using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Refactor
{
    // K0019: Sorry I'm not sure how to do this better without default interface implementation

    // Indicates that whatever inherits this interface is a state manager
    public interface IFSMStateManager_AbstractIdentifier
    {
    }

    // Defines a state manager that maps key values to objects
    public interface IFSMStateManager_Abstract<T, U> : IFSMStateManager_AbstractIdentifier
    {
        public void Init(T firstStateKey);
        public void AddState(T stateKey, U newState);
        public V AddState<V>(T stateKey) where V : U, new();
        public void ChangeState(T stateKey);
        public void Update();
    }

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

    public abstract class FSMStateManager_Abstract<Key, State> : IFSMStateManager_Abstract<Key, State> where State : class
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
            stateHolder.AddState(stateKey, newState);
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
        public abstract void Update();
    }

    public class FSMStateManager_Generic<Key> : FSMStateManager_Abstract<Key, IFSMStateBase<Key>>
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

        public override void Update()
        {
            stateHolder.currentState.OnUpdate();
        }
    }
}