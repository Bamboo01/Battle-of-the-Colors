using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public interface IFSMStateBase
    {
        public void OnEnter();

        public void OnUpdate();

        public void OnExit();
    }
}