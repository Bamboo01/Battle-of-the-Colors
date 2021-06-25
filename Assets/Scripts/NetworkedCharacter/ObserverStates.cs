using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;
using Bamboo.Events;
using Bamboo.Utility;

public class ObserverUtility : MonoBehaviour
{
    public static IEnumerator PlayOneShot(string paramName, Animator _animator)
    {
        _animator.SetBool(paramName, true);
        yield return null;
        _animator.SetBool(paramName, false);
    }
}

public abstract class FSMState_Observer_Base : IFSMStateBase<string>
{
    protected Animator animator;
    protected MonoBehaviour monoBehaviour;

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual string OnUpdate()
    {
        return null;
    }

    public void Setup(Animator anim, MonoBehaviour mono)
    {
        monoBehaviour = mono;
        animator = anim;
    }

}

public class FSMState_Observer_Idle : FSMState_Observer_Base
{
    public override void OnEnter()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isThrowing", false);
    }
}

public class FSMState_Observer_Run : FSMState_Observer_Base
{
    public override void OnEnter()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", true);
    }
}

public class FSMState_Observer_Shoot : FSMState_Observer_Base
{
    public override void OnEnter()
    {
        Debug.Log("Enter pew");
        animator.SetBool("isIdle", false);
        animator.SetBool("isShooting", true);
    }

    public override void OnExit()
    {
        Debug.Log("Exit pew");
        animator.SetBool("isShooting", false);
        animator.SetBool("isIdle", true);
    }
}

public class FSMState_Observer_Throw : FSMState_Observer_Base
{
    public override void OnEnter()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isThrowing", true);
    }

    public override void OnExit()
    {
        animator.SetBool("isThrowing", false);
        animator.SetBool("isIdle", true);
    }
}