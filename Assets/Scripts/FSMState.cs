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
    //��ǰ״̬ID
    public int StateID;
    //״̬ӵ����
    public MonoBehaviour mono;
    //״̬����������
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