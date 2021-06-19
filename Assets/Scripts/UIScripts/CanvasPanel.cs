using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bamboo.UI
{
    public class CanvasPanel : MonoBehaviour
    {
        public bool ignoreOpenOnlyOneCall = false;
        public bool ignoreCloseAllCall = false;

        public string CanvasName { get; private set; }

        private void Awake()
        {
            CanvasName = gameObject.name;
        }

        public virtual void Open()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}


