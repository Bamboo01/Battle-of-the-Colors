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

    //IFSMStateManager_UpdatableBasic<string> stateManager = new FSMStateManager_UpdatableBasic<string>();
    Vector3 lastPosition;

    //State machine booleans
    public bool isRunning;
    public bool isThrowing;
    public bool isShooting;

    /// Remove code that relies on animation states synced by NetworkCharacter.cs

    /*
    void Start()
    {
        FSMState_Observer_Base state = stateManager.AddState<FSMState_Observer_Idle>("idle");
        state.Setup(animator, this);

        state = stateManager.AddState<FSMState_Observer_Run>("run");
        state.Setup(animator, this);

        state = stateManager.AddState<FSMState_Observer_Shoot>("shoot");
        state.Setup(animator, this);

        state = stateManager.AddState<FSMState_Observer_Throw>("throw");
        state.Setup(animator, this);

        stateManager.Init("idle");
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
                stateManager.ChangeState("shoot");
                break;
            case (_, true, _):
                stateManager.ChangeState("throw");
                break;
            case (true, _, _):
                stateManager.ChangeState("run");
                break;
            default:
                // Man why the bone don't rotate
                GunGameObject.transform.localRotation = Quaternion.identity;
                stateManager.ChangeState("idle");
                break;
        }
        stateManager.Update();
    }
    */
}
