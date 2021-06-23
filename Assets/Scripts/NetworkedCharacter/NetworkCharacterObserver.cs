using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;

public class NetworkCharacterObserver : MonoBehaviour
{
    [Header("Gun Alignment")]
    [SerializeField] Transform GunHandBone;
    [SerializeField] GameObject GunGameObject;

    public NetworkCharacter networkCharacter;
    [HideInInspector] public Animator animator;

    FSMStateManager stateManager = new FSMStateManager();
    Vector3 lastPosition;

    //State machine booleans
    public bool isRunning;
    public bool isThrowing;
    public bool isShooting;

    FSMIdleState idleState;
    FSMRunState runState;
    FSMShootState shootState;
    FSMThrowState throwState;

    void Start()
    {
        idleState = stateManager.AddState<FSMIdleState>();
        idleState.Setup(animator, this);

        runState = stateManager.AddState<FSMRunState>();
        runState.Setup(animator, this);

        shootState = stateManager.AddState<FSMShootState>();
        shootState.Setup(animator, this);

        throwState = stateManager.AddState<FSMThrowState>();
        throwState.Setup(animator, this);
    }

    void Update()
    {
        GunGameObject.transform.position = GunHandBone.position;

        isThrowing = networkCharacter.isThrowing;
        isShooting = networkCharacter.isShooting;
        if (lastPosition - transform.position == Vector3.zero)
        {
            isRunning = false;
        }
        else
        {
            isRunning = true;
        }
        lastPosition = transform.position;

        switch (isRunning, isThrowing, isShooting)
        {
            case (_, _, true):
                // Man why the bone don't rotate
                Vector3 rotation = Vector3.zero;
                rotation.y += 90.0f;
                GunGameObject.transform.localRotation = Quaternion.Euler(rotation);
                stateManager.UpdateState(shootState);
                break;
            case (_, true, _):
                stateManager.UpdateState(throwState);
                break;
            case (true, _, _):
                stateManager.UpdateState(runState);
                break;
            default:
                // Man why the bone don't rotate
                GunGameObject.transform.localRotation = Quaternion.identity;
                stateManager.UpdateState(idleState);
                break;
        }
        stateManager.Update();
    }
}
