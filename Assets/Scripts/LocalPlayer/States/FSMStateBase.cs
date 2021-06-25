using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Refactor
{
    // Indicates that whatever inherits this interface is a state
    public interface IFSMState_AbstractIdentifier
    {
    }

    public interface IFSMState_Base : IFSMState_AbstractIdentifier
    {
        public void OnEnter();
        public void OnExit();
    }

    public interface IFSMState_UpdatableBasic<Key> : IFSMState_Base
    {
        // Return value is the next state to change to
        public Key OnUpdate();
    }
}