using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Events;
using Refactor;

public class NetworkCharacterController : MonoBehaviour
{
    [Header("Gun Alignment")]
    [SerializeField] Transform GunHandBone;
    [SerializeField] GameObject GunGameObject;

    [Header("Camera Target")]
    [SerializeField] Transform target;

    [Header("Controller")]
    [SerializeField] CharacterController controller;
    [SerializeField] float playerSpeed = 3.0f;

    [Header("Networking")]
    [SerializeField] NetworkCharacter networkCharacter;

    // Commands
    private Queue<InputCommand> CommandQueue = new Queue<InputCommand>();
    Vector3 resultantDirection;


    void Start()
    {
        // Setting up of various managers
        CameraManager.Instance.AssignTargets(target, target);

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

        GunGameObject.transform.position = GunHandBone.position;
        ProcessInputQueue();
        ProccessPhysics();
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
                    break;
            }
        }

        resultantDirection = resultantDirection.normalized;
        CommandQueue.Clear();
    }

    void ProccessPhysics()
    {
        // Apply Gravity
        controller.SimpleMove(Physics.gravity);

        // Proccess physics
        controller.Move(resultantDirection * playerSpeed * Time.deltaTime);

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
