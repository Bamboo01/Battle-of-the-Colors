using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMangger : ManagerBase<UIMangger>
{
   protected override byte SetMessageType()
    {
        return MessageType.Type_UI;
    }
    void Skill()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {

        }
    }
}
