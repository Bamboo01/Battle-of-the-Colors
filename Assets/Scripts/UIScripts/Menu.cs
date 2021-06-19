using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public bool ignoreOpenOnlyOneCall = false;
    public bool ignoreCloseAllCall = false;
    public string MenuName { get; private set; }

    private void Awake()
    {
        MenuName = gameObject.name;
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
