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

public class FSMIdleState : IFSMStateBase
{
    Animator animator;
    MonoBehaviour monoBehaviour;

    public FSMIdleState()
    {

    }

    public void Setup(Animator anim, MonoBehaviour mono)
    {
        monoBehaviour = mono;
        animator = anim;
    }

    public void OnEnter()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isThrowing", false);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}

public class FSMRunState : IFSMStateBase
{
    Animator animator;
    MonoBehaviour monoBehaviour;

    public FSMRunState()
    {

    }

    public void Setup(Animator anim, MonoBehaviour mono)
    {
        monoBehaviour = mono;
        animator = anim;
    }

    public void OnEnter()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", true);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}

public class FSMShootState : IFSMStateBase
{
    Animator animator;
    MonoBehaviour monoBehaviour;
    public FSMShootState()
    {

    }

    public void Setup(Animator anim, MonoBehaviour mono)
    {
        monoBehaviour = mono;
        animator = anim;
    }

    public void OnEnter()
    {
        Debug.Log("Enter pew");
        animator.SetBool("isIdle", false);
        animator.SetBool("isShooting", true);
    }

    public void OnExit()
    {
        Debug.Log("Exit pew");
        animator.SetBool("isShooting", false);
        animator.SetBool("isIdle", true);
    }

    public void OnUpdate()
    {

    }
}

public class FSMThrowState : IFSMStateBase
{
    Animator animator;
    MonoBehaviour monoBehaviour;

    public FSMThrowState()
    {

    }

    public void Setup(Animator anim, MonoBehaviour mono)
    {
        monoBehaviour = mono;
        animator = anim;
    }

    public void OnEnter()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isThrowing", true);
    }

    public void OnExit()
    {
        animator.SetBool("isThrowing", false);
        animator.SetBool("isIdle", true);
    }

    public void OnUpdate()
    {

    }
}