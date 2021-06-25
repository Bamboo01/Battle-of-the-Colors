using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Events;
using Bamboo.Utility;
using Bamboo.UI;

namespace CSZZGame.Refactor
{
    public class UIManager : Singleton<UIManager>
    {
        NetworkCharacter referencedCharacter;
        GameObject skillContainer;
        protected override void OnAwake()
        {
            _persistent = false;
            base.OnAwake();
        }

        public void SetupUIManager(NetworkCharacter referencedCharacter)
        {

        }

        void Awake()
        {
            referencedCharacter = null;
        }

        // Update is called once per frame
        void Update()
        {
            if (!referencedCharacter)
            {
                return;
            }
        }
    }
}
