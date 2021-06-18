using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    idle,
    run,
    attack,
    climb,
   
}
public abstract class FSMState 
{
    //当前状态ID
    public int StateID;
    //状态拥有者
    public MonoBehaviour mono;
    //状态所属管理器
    public FSMManager fsmManager;
    public FSMState(int stateID,MonoBehaviour mono,FSMManager manager)
    {
        this.StateID = stateID;
        this.mono = mono;
        this.fsmManager = manager;
    }
    public abstract void OnEnter();
    public abstract void OnUpdate();
}