using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase<T> : MonoBase where T:MonoBase
{
    public static T Instance;
    //管理的消息接收者
    public List<MonoBase> ReceiveList = new List<MonoBase>();
    //当前管理类接收的消息类型
    protected byte messageType;

    protected virtual void Awake()
    {
        Instance = this as T;
        //设置消息类型
        messageType = SetMessageType();
        //将当前的管理类添加到消息中心列表中去
        MessageCenter.Managers.Add(this);
    }
    //返回当前管理类的消息类型
    protected virtual byte SetMessageType()
    {
        //默认写的Player
        return MessageType.Type_Player;
    }

    //注册消息监听
    public  void RegisterReceiver(MonoBase mb)
    {
        if (!ReceiveList.Contains(mb))
        {
            ReceiveList.Add(mb);
        }
    }

    //接收到消息，并且向下分发消息
    public override void Receivemessage(Message message)
    {
        base.Receivemessage(message);
        //如果当前消息不匹配，则不向下分发消息
        if (message.Type != messageType)
        {
            return;
        }
        foreach(MonoBase mb in ReceiveList)
        {
            mb.Receivemessage(message);
        }
    }

}
