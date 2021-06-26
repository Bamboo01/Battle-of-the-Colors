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
            if (Input.GetKeyDown(KeyCode.LeftShift) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.STEALTH_START);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.STEALTH_STOP);
            }
            if (Input.GetMouseButton(0) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.FIRE);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.SKILL1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.SKILL2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.SKILL3);
            }
        }
    }
}
