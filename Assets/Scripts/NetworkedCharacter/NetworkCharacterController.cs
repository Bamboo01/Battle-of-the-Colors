using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Events;
using CSZZGame.Networking;
using CSZZGame.Refactor;
using CSZZGame.Character.Movement;

public class NetworkCharacterController : MonoBehaviour
{
    [Header("Camera Target")]
    [SerializeField] Transform target;

    [Header("Controller")]
    [SerializeField] CharacterController controller;
    [SerializeField] float playerSpeed = 3.0f;

    [Header("Networking")]
    [SerializeField] NetworkCharacter networkCharacter;

    // Movement
    IFSMStateManager_Movement movementStateManager = new FSMStateManager_Movement();

    // Commands
    private Queue<InputCommand> CommandQueue = new Queue<InputCommand>();
    Vector3 resultantDirection;


    void Start()
    {
        // Setting up of various managers
        CameraManager.Instance.AssignTargets(target, target);

        // Setup movement
        movementStateManager.SetPlayerSettings(controller, playerSpeed);

        movementStateManager.AddState<FSMState_Movement_Normal>("normal");

        movementStateManager.Init("normal");

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

        ProcessInputQueue();
        movementStateManager.Update(resultantDirection);

        UpdateTransform();
    }

    void ProcessInputQueue()
    {
        resultantDirection = Vector3.zero;

        // Get forward direction vector
        Vector3 frontDir = target.transform.forward;
        frontDir = new Vector3(frontDir.x, 0, frontDir.z).normalized;

        // Get right direction vector
        Vector3 rightDir = target.transform.right;
        rightDir = new Vector3(rightDir.x, 0, rightDir.z).normalized;

        // Lazy af, shooting
        bool localShooting = false;

        while (CommandQueue.Count != 0)
        {
            var command = CommandQueue.Dequeue();
            switch (command)
            {
                case InputCommand.MOVE_FORWARD:
                    resultantDirection += frontDir;
                    break;
                case InputCommand.MOVE_BACKWARDS:
                    resultantDirection -= frontDir;
                    break;
                case InputCommand.STRAFE_LEFT:
                    resultantDirection -= rightDir;
                    break;
                case InputCommand.STRAFE_RIGHT:
                    resultantDirection += rightDir;
                    break;
                case InputCommand.FIRE:
                    networkCharacter.CmdFireBullet();
                    localShooting = true;
                    break;
                case InputCommand.SHIELD:
                    networkCharacter.CmdSpawnShield();
                    break;
            }
        }

        if (networkCharacter.isShooting != localShooting)
        {
            networkCharacter.isShooting = localShooting;
        }
        resultantDirection = resultantDirection.normalized;
        CommandQueue.Clear();
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

    void OnInputEvent(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<InputCommand> info = (EventRequestInfo<InputCommand>)eventRequestInfo;
        CommandQueue.Enqueue(info.body);
    }
}
