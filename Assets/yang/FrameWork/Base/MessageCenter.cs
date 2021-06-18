using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : MonoBehaviour
{
    //管理类集合
    public static List<MonoBase> Managers = new List<MonoBase>();

    //发送消息
    public static void SendMessage(Message msg)
    {
        foreach(MonoBase mb in Managers)
        {
            mb.Receivemessage(msg);
        }
    }

    public static void SendMessage(byte type,int command,object content)
    {
        Message msg = new Message(type, command, content);
        SendMessage(msg);
    }
}
