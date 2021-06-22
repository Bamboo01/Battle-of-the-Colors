using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Refactor
{
    public class FSMStateManager
    {
        private Dictionary<IFSMStateBase, IFSMStateBase> stateDictionary = new Dictionary<IFSMStateBase, IFSMStateBase>();
        private IFSMStateBase currentState = null;

        public void Init(IFSMStateBase firstState)
        {
            UpdateState(firstState);
        }

        public void UpdateState(IFSMStateBase nextState)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }
            IFSMStateBase newState = stateDictionary[nextState];
            if (currentState == newState)
            {
                return;
            }
            else
            {
                currentState = stateDictionary[nextState];
                currentState.OnEnter();
            }
        }

        public void AddState(IFSMStateBase newState)
        {
            stateDictionary.Add(newState, newState);
        }

        public T AddState<T>() where T : IFSMStateBase, new()
        {
            T newState = new T();
            stateDictionary.Add(newState, newState);
            return newState;
        }

        public void Update()
        {
            currentState.OnUpdate();
        }
    }
}