using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManager 
{
    //状态列表
    public List<FSMState> stateList = new List<FSMState>();
    //当前状态
    public int CurrentIndex = -1;
    //改变状态
    public void ChangeState(int StateID)
    {
        CurrentIndex = StateID;
        stateList[CurrentIndex].OnEnter();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (CurrentIndex != -1)
        {
            stateList[CurrentIndex].OnUpdate();
        }
        
    }
}