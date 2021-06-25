using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Refactor
{
    // Indicates that whatever inherits this interface is a state
    public interface IFSMState_AbstractIdentifier
    {
    }

    public interface IFSMStateBase<Key> : IFSMState_AbstractIdentifier
    {
        public void OnEnter();

        public Key OnUpdate();

        public void OnExit();
    }
}