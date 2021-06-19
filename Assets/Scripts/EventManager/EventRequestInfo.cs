using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bamboo.Events
{
    // Template magic to allow custom types to be stored inside the Event Channels
    public interface IEventRequestInfo
    {
        string path { get; }
        object sender { get; }
    }

    public class EventRequestInfo<T> : IEventRequestInfo
    {
        // Event Channel Name
        // 棰戦亾鐨勫悕璇?
        public string path { get; private set; }

        // Event Request Sender (Can be typecasted)
        // 娑堟伅鐨勫彂浠朵汉 锛堣兘琚被鍨嬭浆鎹級
        public object sender { get; private set; }

        // Contains the custom data of the info
        // 鑷璧勬枡
        public T body;

        public EventRequestInfo(string channelpath, object senderobject, T requestbody)
        {
            path = channelpath;
            sender = senderobject;
            body = requestbody;
        }
    }

}