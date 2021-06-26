using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;

namespace CSZZGame.Character
{
    public interface IFSMStateManager_Character : Refactor.Internal.I_IFSMStateManager_Basic<FSMState_Character_Type, IFSMState_Character_Base>
    {
        public FSMState_Character_Type currentStateType { get; }

        public void SetPlayerSettings(PlayerSettings playerSettings);
        public void Update(Vector3 desiredMovement);
    }


    public class FSMStateManager_Character : Refactor.Internal.I_FSMStateManager_Basic<FSMState_Character_Type, IFSMState_Character_Base>, IFSMStateManager_Character
    {
        private PlayerSettings playerSettings;

        // Return the current state's type if it is valid, otherwise NULL
        public FSMState_Character_Type currentStateType => stateHolder.currentState?.type ?? FSMState_Character_Type.NULL;

        public void SetPlayerSettings(PlayerSettings playerSettings)
        {
            this.playerSettings = playerSettings;
        }

        public override void AddState(FSMState_Character_Type stateKey, IFSMState_Character_Base newState)
        {
            newState.playerSettings = playerSettings;
            base.AddState(stateKey, newState);
        }

        public void Update(Vector3 desiredMovement)
        {
            stateHolder.currentState.OnUpdate(desiredMovement);
        }
    }
}