using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Utility;

namespace Bamboo.UI
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField] private Menu[] Menus;
        private Dictionary<string, GameObject> NameToGameObject;

        protected override void OnAwake()
        {
            _persistent = false;
        }

        private void Start()
        {
            NameToGameObject = new Dictionary<string, GameObject>();
            foreach (Menu a in Menus)
            {
                Debug.Log("Added a menu: " + a.name);
                NameToGameObject.Add(a.name, a.gameObject);
            }
        }

        public void OpenMenu(string menuName)
        {
            foreach (Menu a in Menus)
            {
                if (a.name == menuName)
                {
                    a.Open();
                }
            }
        }

        public void OnlyOpenThisMenu(string menuName)
        {
            foreach (Menu a in Menus)
            {
                if (a.name == menuName)
                {
                    a.Open();
                }
                else
                {
                    if (a.ignoreOpenOnlyOneCall)
                    {
                        continue;
                    }
                    a.Close();
                }
            }
        }


        public void CloseMenu(string menuName)
        {
            foreach (Menu a in Menus)
            {
                if (a.name == menuName)
                {
                    a.Close();
                }
            }
        }
        public void CloseAllMenus()
        {
            foreach (Menu a in Menus)
            {
                if (a.ignoreCloseAllCall)
                {
                    continue;
                }
                a.Close();
            }
        }


        public GameObject getMenuGameObject(string menuName)
        {
            GameObject a;
            NameToGameObject.TryGetValue(menuName, out a);
            return a;
        }
    }
}