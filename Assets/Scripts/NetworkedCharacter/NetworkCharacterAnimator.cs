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
            case Action_Character_Type.STEALTH:
                {
                    var actionStealth = action.body as Action_Character_Stealth;
                    animator.SetBool("isStealthing", actionStealth.startStealth);
                }
                break;
            case Action_Character_Type.SHOOT:
                animator.SetBool("isShooting", true);
                shotThisFrame = true;
                break;
            case Action_Character_Type.SKILL:
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
    }
}
