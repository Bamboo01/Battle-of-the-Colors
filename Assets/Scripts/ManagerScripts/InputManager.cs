using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bamboo.Utility;
using Bamboo.Events;

namespace CSZZGame.Refactor
{
    public class InputManager : Singleton<InputManager>
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.W) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.MOVE_FORWARD);
            }
            if (Input.GetKey(KeyCode.S) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.MOVE_BACKWARDS);
            }
            if (Input.GetKey(KeyCode.A) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.STRAFE_LEFT);
            }
            if (Input.GetKey(KeyCode.D) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.STRAFE_RIGHT);
            }
            if (Input.GetKey(KeyCode.LeftShift) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.STEALTH_START);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.STEALTH_STOP);
            }
            if (Input.GetKeyDown(KeyCode.W) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.TAP_FORWARD);
            }
            if (Input.GetKeyDown(KeyCode.S) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.TAP_BACKWARDS);
            }
            if (Input.GetKeyDown(KeyCode.A) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.TAP_LEFT);
            }
            if (Input.GetKeyDown(KeyCode.D) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.TAP_RIGHT);
            }
            if (Input.GetKeyDown(KeyCode.Space) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.LAUNCH_STRATEGEM);
            }
            if (Input.GetMouseButton(0) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.FIRE);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.STRATEGEM_MODE);
            }
        }
    }
}
