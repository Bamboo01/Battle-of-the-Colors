using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public class FSMStateManager
    {
        private Dictionary<string, IFSMStateBase> stateDictionary = new Dictionary<string, IFSMStateBase>();

        public string currentStateName { private set; get; }
        public string nextStateName { private set; get; }

        private IFSMStateBase currentState;

        public void Init(IFSMStateBase baseState)
        {
            currentStateName = baseState.StateName;
            nextStateName = baseState.StateName;

            baseState.OnEnter();
        }

        public void Update()
        {
            if (currentStateName.Length == 0)
            {
                Debug.LogError("Current state is empty!");
                return;
            }

            if (currentStateName == nextStateName)
            {
                currentState.OnUpdate();
            }
            else
            {
                if (nextStateName.Length == 0)
                {
                    Debug.LogError("Next state is empty!");
                    return;
                }

                IFSMStateBase newcurrentState;
                if (!stateDictionary.TryGetValue(nextStateName, out newcurrentState))
                {
                    Debug.LogError("Invalid state name: " + nextStateName);
                }

                currentState = newcurrentState;
                currentState.OnEnter();

                currentStateName = currentState.StateName;
                nextStateName = currentState.StateName;
            }
        }

        public void ChangeState(IFSMStateBase state)
        {
            nextStateName = state.StateName;
        }

        public void AddState(IFSMStateBase state)
        {
            stateDictionary.Add(state.StateName, state);
        }
    }
}

