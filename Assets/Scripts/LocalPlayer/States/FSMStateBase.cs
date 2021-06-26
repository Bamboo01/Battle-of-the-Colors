using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Refactor
{
    public interface IFSMState_Base
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