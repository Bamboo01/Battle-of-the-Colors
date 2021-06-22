using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;
using Bamboo.Events;
using Bamboo.Utility;

public class FSMIdleState : IFSMStateBase
{
    Animator animator;

    public FSMIdleState()
    {

    }

    public void Setup(Animator anim)
    {
        animator = anim;
    }

    public void OnEnter()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
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

    public FSMRunState()
    {

    }

    public void Setup(Animator anim)
    {
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

    public FSMShootState()
    {

    }

    bool updatedOnce = false;

    public void Setup(Animator anim)
    {
        animator = anim;
    }

    public void OnEnter()
    {
        animator.SetBool("isShooting", true);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if (!updatedOnce)
        {
            animator.SetBool("isShooting", false);
        }
    }
}

public class FSMThrowState : IFSMStateBase
{
    Animator animator;

    bool updatedOnce = false;

    public FSMThrowState()
    {

    }

    public void Setup(Animator anim)
    {
        animator = anim;
    }

    public void OnEnter()
    {
        animator.SetBool("isThrowing", true);
    }

    public void OnExit()
    {
        updatedOnce = false;
    }

    public void OnUpdate()
    {
        if (!updatedOnce)
        {
            animator.SetBool("isThrowing", false);
            updatedOnce = true;
        }
    }
}