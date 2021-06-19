using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public interface IFSMStateBase
    {
        public string StateName { get; }

        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
    }

    public abstract class FSMStateBase<T> : IFSMStateBase
    {
        private static string name;

        public string StateName 
        {
            private set => StateName = value;
            get => StateName;
        }

        public FSMStateBase()
        {
            if (name.Length == 0)
            {
                name = GetType().Name;
            }
            StateName = name;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();
    }

}