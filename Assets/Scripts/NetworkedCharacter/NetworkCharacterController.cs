using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Events;
using CSZZGame.Networking;
using CSZZGame.Refactor;
using CSZZGame.Character;

public class NetworkCharacterController : MonoBehaviour
{
    [Header("Camera Target")]
    [SerializeField] Transform target;

    [Header("Controller")]
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerSettings playerSettings;

    [Header("Networking")]
    [SerializeField] NetworkCharacter networkCharacter;

    // Character State (Movement, etc.)
    IFSMStateManager_Character characterStateManager = new FSMStateManager_Character();
    Vector3 resultantDirection = Vector3.zero;

    // Commands
    private Queue<IAction_Character> actionQueue = new Queue<IAction_Character>();

    void Start()
    {
        // Setting up of various managers
        CameraManager.Instance.AssignTargets(target, target);

        // Setup movement
        playerSettings.networkCharacter = networkCharacter;
        playerSettings.characterController = controller;
        playerSettings.cameraTarget = target;
        characterStateManager.SetPlayerSettings(playerSettings);

        characterStateManager.AddState<FSMState_Character_Normal>(FSMSTATE_CHARACTER_TYPE.NORMAL);
        characterStateManager.AddState<FSMState_Character_StealthNormal>(FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL);

        characterStateManager.Init(FSMSTATE_CHARACTER_TYPE.NORMAL);

        // Setting up of event listeners
        EventManager.Instance.Listen(EventChannels.OnInputEvent, OnInputEvent);
    }

    void Update()
    {
        if (!networkCharacter.hasAuthority)
        {
            this.enabled = false;
            return;
        }

        ProcessActions();

        characterStateManager.Update(resultantDirection);
        resultantDirection = Vector3.zero;

        UpdateTransform();
    }

    private void ProcessActions()
    {
        while (actionQueue.Count > 0)
        {
            IAction_Character action = actionQueue.Dequeue();

            if (!action.IsActionAllowed(characterStateManager.currentStateType))
                continue;

            switch (action.identifier)
            {
                case ACTION_CHARACTER_TYPE.MOVE:
                    {
                        var actionMove = action as Action_Character_Move;
                        resultantDirection += actionMove.desiredMovement;
                    }
                    break;
                case ACTION_CHARACTER_TYPE.STEALTH:
                    {
                        var actionStealth = action as Action_Character_Stealth;
                        characterStateManager.ChangeState(actionStealth.startStealth ? FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL : FSMSTATE_CHARACTER_TYPE.NORMAL);
                    }
                    break;
                case ACTION_CHARACTER_TYPE.SHOOT:
                    {
                        networkCharacter.CmdFireBullet();
                    }
                    break;
                case ACTION_CHARACTER_TYPE.SKILL:
                    {
                        var actionSkill = action as Action_Character_Skill;
                        switch (actionSkill.skillID)
                        {
                            case 0:
                                StrategemManager.Instance.strategemActivation();
                                break;
                            case 1:
                                StrategemManager.Instance.strategemDeactivation();
                                break;
                        }
                    }
                    break;
                case ACTION_CHARACTER_TYPE.LAUNCH_STRATEGEM:
                    networkCharacter.CmdSpawnSkill(StrategemManager.Instance.getCallableStrategem());
                    break;
            }

            EventManager.Instance.Publish<IAction_Character>(EventChannels.OnOwnCharacterActionEvent, this, action);
        }

        actionQueue.Clear();
    }

    void UpdateTransform()
    {
        // Debug
        // Get direction of player to character
        Vector3 cameraDirection = (transform.position - CameraManager.Instance.GetCameraTransform().position).normalized;
        cameraDirection.y = transform.position.y;
        Vector3 finalRotation = Quaternion.LookRotation(cameraDirection, Vector3.up).eulerAngles;
        finalRotation.x = 0;
        finalRotation.z = 0;
        // Why da otter be rotatin :(
        transform.rotation = Quaternion.Euler(finalRotation);
    }

    // Queue up an action when an input is received
    void OnInputEvent(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<InputCommand> info = (EventRequestInfo<InputCommand>)eventRequestInfo;

        switch (info.body)
        {
            case InputCommand.MOVE_FORWARD:
            case InputCommand.MOVE_BACKWARDS:
                {
                    Vector3 frontDir = target.transform.forward;
                    frontDir = new Vector3(frontDir.x, 0, frontDir.z).normalized;

                    actionQueue.Enqueue(
                        new Action_Character_Move(info.body == InputCommand.MOVE_FORWARD ? frontDir : -frontDir)
                    );
                }
                break;
            case InputCommand.STRAFE_LEFT:
            case InputCommand.STRAFE_RIGHT:
                {
                    Vector3 rightDir = target.transform.right;
                    rightDir = new Vector3(rightDir.x, 0, rightDir.z).normalized;

                    actionQueue.Enqueue(
                        new Action_Character_Move(info.body == InputCommand.STRAFE_RIGHT ? rightDir : -rightDir)
                    );
                }
                break;
            case InputCommand.STEALTH_START:
                actionQueue.Enqueue(new Action_Character_Stealth(true));
                break;
            case InputCommand.STEALTH_STOP:
                actionQueue.Enqueue(new Action_Character_Stealth(false));
                break;
            case InputCommand.FIRE:
                actionQueue.Enqueue(new Action_Character_Shoot());
                break;
            case InputCommand.SKILL1:
                actionQueue.Enqueue(new Action_Character_Skill(0));
                break;
            case InputCommand.SKILL2:
                actionQueue.Enqueue(new Action_Character_Skill(1));
                break;
            case InputCommand.SKILL3:
                actionQueue.Enqueue(new Action_Character_Skill(2));
                break;
            case InputCommand.LAUNCH_STRATEGEM:
                actionQueue.Enqueue(new Action_Character_LaunchStrategem());
                break;
        }
    }
}
