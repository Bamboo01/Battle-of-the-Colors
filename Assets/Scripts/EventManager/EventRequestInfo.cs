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

    [System.Serializable]
    public class EventRequestInfo<T> : IEventRequestInfo
    {
        public string path { get; private set; }

        public object sender { get; private set; }

        public T body;

        public EventRequestInfo(string channelpath, object senderobject, T requestbody)
        {
            path = channelpath;
            sender = senderobject;
            body = requestbody;
        }
    }

}