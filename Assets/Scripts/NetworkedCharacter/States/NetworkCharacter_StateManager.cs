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

        public void SetPlayerSettings(PlayerSettings playerSettings, PlayerData playerData);
        public void Update(Vector3 desiredMovement);
    }


    public class FSMStateManager_Character : FSMStateManager_Basic<FSMSTATE_CHARACTER_TYPE>, IFSMStateManager_Character
    {
        protected new IFSMState_Character_Base currentState { get => base.currentState as IFSMState_Character_Base; set => base.currentState = value; }

        private PlayerSettings playerSettings;
        private PlayerData playerData;

        // Return the current state's type if it is valid, otherwise NULL
        public FSMSTATE_CHARACTER_TYPE currentStateType => currentState?.type ?? FSMSTATE_CHARACTER_TYPE.NULL;

        public void SetPlayerSettings(PlayerSettings playerSettings, PlayerData playerData)
        {
            this.playerSettings = playerSettings;
            this.playerData = playerData;
        }

        protected override bool CheckStateType(IFSMState_Base state)
        {
            return state as IFSMState_Character_Base != null;
        }
        protected override void SetupAddedState(IFSMState_Base newState)
        {
            IFSMState_Character_Base state = newState as IFSMState_Character_Base;

            state.playerSettings = playerSettings;
            state.playerData = playerData;
        }

        public override void ChangeState(FSMSTATE_CHARACTER_TYPE stateKey)
        {
            base.ChangeState(stateKey);

            EventManager.Instance.Publish(EventChannels.OnOwnCharacterStateChangeEvent, this, currentState);
        }
        protected override void OnStateEnter(IFSMState_Base previousState, IFSMState_Base currentState)
        {
            IFSMState_Character_Base currentStateChar = currentState as IFSMState_Character_Base;
            IFSMState_Character_Base previousStateChar = previousState as IFSMState_Character_Base;

            currentStateChar.OnEnter(previousStateChar?.type ?? FSMSTATE_CHARACTER_TYPE.NULL);
        }
        protected override void OnStateExit(IFSMState_Base currentState, IFSMState_Base nextState)
        {
            IFSMState_Character_Base currentStateChar = currentState as IFSMState_Character_Base;
            IFSMState_Character_Base nextStateChar = nextState as IFSMState_Character_Base;

            currentStateChar.OnExit(nextStateChar.type);
        }

        public void Update(Vector3 desiredMovement)
        {
            float remainingMovementDist = desiredMovement.magnitude;
            Vector3 desiredMovementDir = desiredMovement / remainingMovementDist;

            do
            {
                var nextStateKey = currentState.OnUpdate(desiredMovementDir, remainingMovementDist, out remainingMovementDist);

                if (nextStateKey == FSMSTATE_CHARACTER_TYPE.NULL)
                    break;
                else if (nextStateKey != currentStateKey)
                    ChangeState(nextStateKey);
            }
            while (remainingMovementDist > -0.5f);
        }
    }
}