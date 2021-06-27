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
}