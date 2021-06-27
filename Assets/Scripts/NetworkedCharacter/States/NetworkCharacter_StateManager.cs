using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;

namespace CSZZGame.Character
{
    public interface IFSMStateManager_Character : IFSMStateManager_Basic<FSMState_Character_Type>
    {
        public FSMState_Character_Type currentStateType { get; }

        public void SetPlayerSettings(PlayerSettings playerSettings);
        public void Update(Vector3 desiredMovement);
    }


    public class FSMStateManager_Character : FSMStateManager_Basic<FSMState_Character_Type>, IFSMStateManager_Character
    {
        /// <summary>
        /// Get the currently active state object.
        /// <para>
        /// Replace this accessor with your preferred state class when extending this class.
        /// </para>
        /// </summary>
        protected new IFSMState_Character_Base currentState { get => currentStateAsBase as IFSMState_Character_Base; set => currentStateAsBase = value; }

        private PlayerSettings playerSettings;

        // Return the current state's type if it is valid, otherwise NULL
        public FSMState_Character_Type currentStateType => currentState?.type ?? FSMState_Character_Type.NULL;

        public void SetPlayerSettings(PlayerSettings playerSettings)
        {
            this.playerSettings = playerSettings;
        }

        protected override bool CheckStateType(IFSMState_Base state)
        {
            return state as IFSMState_Character_Base != null;
        }
        protected override void SetupAddedState(IFSMState_Base newState)
        {
            IFSMState_Character_Base state = newState as IFSMState_Character_Base;

            state.playerSettings = playerSettings;
        }

        public void Update(Vector3 desiredMovement)
        {
            currentState.OnUpdate(desiredMovement);
        }
    }
}