using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Refactor;

public class NetworkCharacterObserver : MonoBehaviour
{
    public NetworkCharacter networkCharacter;
    public Animator animator;

    FSMStateManager stateManager = new FSMStateManager();
    Vector3 lastPosition;

    //State machine booleans
    bool isRunning;
    bool isThrowing;
    bool isShooting;

    FSMIdleState idleState;
    FSMRunState runState;
    FSMShootState shootState;
    FSMThrowState throwState;

    void Start()
    {
        idleState = stateManager.AddState<FSMIdleState>();
        idleState.Setup(animator);

        runState = stateManager.AddState<FSMRunState>();
        runState.Setup(animator);

        shootState = stateManager.AddState<FSMShootState>();
        shootState.Setup(animator);

        throwState = stateManager.AddState<FSMThrowState>();
        throwState.Setup(animator);
    }

    void Update()
    {
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
            case (true, _, _):
                stateManager.UpdateState(runState);
                break;
            case (_, true, _):
                stateManager.UpdateState(throwState);
                break;
            case (_, _, true):
                stateManager.UpdateState(shootState);
                break;
            case (_, _, _):
                stateManager.UpdateState(idleState);
                break;
        }

        
        stateManager.Update();
    }
}
