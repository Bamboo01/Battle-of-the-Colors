using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;
using Bamboo.Events;

namespace CSZZGame.Character
{
    public interface IFSMStateManager_Character : IFSMStateManager_Basic<FSMSTATE_CHARACTER_TYPE>
    {
        public FSMSTATE_CHARACTER_TYPE currentStateType { get; }

        public void SetPlayerSettings(PlayerSettings playerSettings);
        public void Update(Vector3 desiredMovement);
    }


    public class FSMStateManager_Character : FSMStateManager_Basic<FSMSTATE_CHARACTER_TYPE>, IFSMStateManager_Character
    {
        protected new IFSMState_Character_Base currentState { get => base.currentState as IFSMState_Character_Base; set => base.currentState = value; }

        private PlayerSettings playerSettings;

        // Return the current state's type if it is valid, otherwise NULL
        public FSMSTATE_CHARACTER_TYPE currentStateType => currentState?.type ?? FSMSTATE_CHARACTER_TYPE.NULL;

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

        public override void ChangeState(FSMSTATE_CHARACTER_TYPE stateKey)
        {
            base.ChangeState(stateKey);

            EventManager.Instance.Publish(EventChannels.OnOwnCharacterStateChangeEvent, this, currentState);
        }

        public void Update(Vector3 desiredMovement)
        {
            currentState.OnUpdate(desiredMovement);
        }
    }
}