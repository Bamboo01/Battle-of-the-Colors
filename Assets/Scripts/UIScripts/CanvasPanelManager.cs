using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Utility;


namespace Bamboo.UI
{
    public class CanvasPanelManager : Singleton<CanvasPanelManager>
    {
        [SerializeField] private CanvasPanel[] CanvasPanels;
        private Dictionary<string, GameObject> NameToGameObject;

        protected override void OnAwake()
        {
            _persistent = false;
        }

        public void Start()
        {
            NameToGameObject = new Dictionary<string, GameObject>();
            foreach (CanvasPanel a in CanvasPanels)
            {
                Debug.Log("Added a canvas panel: " + a.name);
                NameToGameObject.Add(a.name, a.gameObject);
            }
        }

        public void OpenCanvasPanel(string Name)
        {
            foreach (CanvasPanel a in CanvasPanels)
            {
                if (a.name == Name)
                {
                    a.Open();
                }
            }
        }

        public void OnlyOpenThisCanvasPanel(string Name)
        {
            foreach (CanvasPanel a in CanvasPanels)
            {
                if (a.name == Name)
                {
                    a.Open();
                }
                else
                {
                    a.Close();
                }
            }
        }


        public void CloseCanvasPanel(string Name)
        {
            foreach (CanvasPanel a in CanvasPanels)
            {
                if (a.name == Name)
                {
                    a.Close();
                }
            }
        }
        public void CloseAllCanvasPanels()
        {
            foreach (CanvasPanel a in CanvasPanels)
            {
                a.Close();
            }
        }

        public GameObject getCanvasPanelGameObject(string Name)
        {
            GameObject a;
            NameToGameObject.TryGetValue(Name, out a);
            return a;
        }
    }

}