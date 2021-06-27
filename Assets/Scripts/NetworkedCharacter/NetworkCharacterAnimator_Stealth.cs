using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Bamboo.Events;
using CSZZGame.Refactor;
using CSZZGame.Character;

public class NetworkCharacterAnimator_Stealth : NetworkBehaviour
{
    [SerializeField]
    private GameObject modelGameObject;
    [SerializeField]
    private GameObject stealthParticleEmitter;

    [SyncVar(hook = nameof(SyncStealthState))]
    private bool isStealth = false;
    private bool localIsStealth = false;
    private bool thisFrameStealth = false;

    public override void OnStartAuthority()
    {
        EventManager.Instance.Listen(EventChannels.OnOwnCharacterStateChangeEvent, OnCharacterStateChangeEvent);
    }
    public override void OnStopAuthority()
    {
        EventManager.Instance.Close(EventChannels.OnOwnCharacterStateChangeEvent, OnCharacterStateChangeEvent);
    }

    private void OnCharacterStateChangeEvent(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<IFSMState_Character_Base> state = eventRequestInfo as EventRequestInfo<IFSMState_Character_Base>;

        switch (state.body.type)
        {
            case FSMSTATE_CHARACTER_TYPE.STEALTH_NORMAL:
            case FSMSTATE_CHARACTER_TYPE.STEALTH_CLIMB:
                thisFrameStealth = true;
                break;
            default:
                thisFrameStealth = false;
                break;
        }
    }

    private void LateUpdate()
    {
        if (!hasAuthority)
            return;

        if (thisFrameStealth != localIsStealth)
        {
            localIsStealth = thisFrameStealth;

            CmdOnStealthStateChange(localIsStealth);
            SetModelVisibility(localIsStealth);
        }
    }

    [Command]
    private void CmdOnStealthStateChange(bool isStealth)
    {
        this.isStealth = isStealth;
        SetModelVisibility(isStealth);
    }

    private void SyncStealthState(bool oldState, bool newState)
    {
        if (hasAuthority)
            return;

        SetModelVisibility(newState);
    }

    private void SetModelVisibility(bool isStealth)
    {
        modelGameObject.SetActive(!isStealth);
        stealthParticleEmitter.SetActive(isStealth);
    }
}
