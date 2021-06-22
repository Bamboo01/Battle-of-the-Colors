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
            if (Input.GetMouseButton(0) == true)
            {
                EventManager.Instance.Publish(EventChannels.OnInputEvent, this, InputCommand.FIRE);
            }
        }
    }
}
