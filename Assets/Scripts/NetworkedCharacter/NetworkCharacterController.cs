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
        playerSettings.characterController = controller;
        characterStateManager.SetPlayerSettings(playerSettings);

        characterStateManager.AddState<FSMState_Character_Normal>(FSMState_Character_Type.NORMAL);
        characterStateManager.AddState<FSMState_Character_StealthNormal>(FSMState_Character_Type.STEALTH_NORMAL);

        characterStateManager.Init(FSMState_Character_Type.NORMAL);

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
                case Action_Character_Type.MOVE:
                    {
                        var actionMove = action as Action_Character_Move;
                        resultantDirection += actionMove.desiredMovement;
                    }
                    break;
                case Action_Character_Type.STEALTH:
                    {
                        var actionStealth = action as Action_Character_Stealth;
                        characterStateManager.ChangeState(actionStealth.startStealth ? FSMState_Character_Type.STEALTH_NORMAL : FSMState_Character_Type.NORMAL);
                    }
                    break;
                case Action_Character_Type.SHOOT:
                    {
                        networkCharacter.CmdFireBullet();
                    }
                    break;
                case Action_Character_Type.SKILL:
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
                case Action_Character_Type.LAUNCH_STRATEGEM:
                    networkCharacter.CmdSpawnSkill(StrategemManager.Instance.getCallableStrategem());
                    break;
            }

            EventManager.Instance.Publish<IAction_Character>(EventChannels.OnOwnCharacterActionEvent, this, action);
        }

        actionQueue.Clear();
    }

    /*
    // Will change to be more extensible
    // TODO: Move movement logic to NetworkCharacter_MovementStates.cs
    void MoveCharacterStealth()
    {
        Vector3 originalPosition = transform.position;
        Vector3 lastValidPosition = originalPosition; // Use to return to last valid stealth position if we end up at an invalid position
        CollisionFlags collisionFlags; // Use to detect if he hit a wall

        if (!isClimbing)
        {
            // We should be on the ground, so move normally
            collisionFlags = controller.Move(resultantDirection * playerSpeed * Time.deltaTime);

            if ((collisionFlags & CollisionFlags.CollidedSides) != 0)
            {

            }
        }
    }
    bool CheckValidStealthPosition(Vector3 position, Vector3 up)
    {
        RaycastHit raycastHit;

        // Get the surface underneath us
        if (Physics.Raycast(position + up, -up, out raycastHit, 2.0f))
        {
            Paintable paintable = raycastHit.transform.GetComponent<Paintable>();
            if (paintable)
            {
                int x = (int)((float)paintable.textureSize * raycastHit.textureCoord.x);
                int y = (int)((float)paintable.textureSize * raycastHit.textureCoord.y);

                RenderTexture.active = paintable.rawmaskcolorTexture;
                pixelSampler.ReadPixels(new Rect(x, (int)paintable.textureSize - y, 1, 1), 0, 0, true);
                pixelSampler.Apply();
            }
            else // This surface is not paintable - no way we can stealth through it
                return false;
        }
        else // No hit, we are in the air
            return true;
    }
    */

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
