using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Events;
using CSZZGame.Refactor;
using CSZZGame.Character;

public class NetworkCharacterAnimator : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] Animator animator;

    // Previously: https://i.imgur.com/qQjQ6o8.png
    // I also don't know how to make this better lol
    private bool movedThisFrame = false;
    private bool shotThisFrame = false;

    private void Start()
    {
        EventManager.Instance.Listen(EventChannels.OnOwnCharacterActionEvent, OnActionEvent);
        EventManager.Instance.Listen(EventChannels.OnOwnCharacterStateChangeEvent, OnCharacterStateChangeEvent);
    }

    private void OnActionEvent(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<IAction_Character> action = eventRequestInfo as EventRequestInfo<IAction_Character>;

        switch (action.body.identifier)
        {
            case Action_Character_Type.MOVE:
                animator.SetBool("isRunning", true);
                movedThisFrame = true;
                break;
            /// Do not listen to stealth action events, in case we are not on paint that we can stealth on
            /// Listen to character state changes instead
            //case Action_Character_Type.STEALTH:
            //    break;
            case Action_Character_Type.SHOOT:
                animator.SetBool("isShooting", true);
                shotThisFrame = true;
                break;
            case Action_Character_Type.SKILL:
                break;
        }
    }
    private void OnCharacterStateChangeEvent(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<IFSMState_Character_Base> state = eventRequestInfo as EventRequestInfo<IFSMState_Character_Base>;

        switch (state.body.type)
        {
            case FSMState_Character_Type.STEALTH_NORMAL:
            case FSMState_Character_Type.STEALTH_CLIMB:
                animator.SetBool("isStealthing", true);
                break;
            default:
                animator.SetBool("isStealthing", false);
                break;
        }
    }

    private void LateUpdate()
    {
        if (!movedThisFrame)
            animator.SetBool("isRunning", false);
        if (!shotThisFrame)
            animator.SetBool("isShooting", false);

        movedThisFrame = false;
        shotThisFrame = false;
    }

    private void OnDestroy()
    {
        EventManager.Instance.Close(EventChannels.OnOwnCharacterActionEvent, OnActionEvent);
        EventManager.Instance.Close(EventChannels.OnOwnCharacterStateChangeEvent, OnCharacterStateChangeEvent);
    }
}
