using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBase : MonoBehaviour
{
    //发送消息
    public static void SendCustomMessage(Message msg)
    {
        MessageCenter.SendMessage(msg);
    }
    public void SendCustomMessage(byte type,int command,object content)
    {
        MessageCenter.SendMessage(type, command, content);
    }
    //接收消息
   public virtual void Receivemessage(Message message)
    {

    }
}
