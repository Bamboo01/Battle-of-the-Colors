using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bamboo.Events
{
    public class EventChannel : UnityEvent<IEventRequestInfo> { }

    public class EventManager : MonoBehaviour
    {
        // Singleton
        // 鍗曚緥妯″紡
        static public EventManager Instance
        {
            get;
            private set;
        }
        public void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("Event Manager instance already created. Deleting it and instantiating a new instance...");
                Destroy(Instance);
                Instance = this;
            }
            else
            {
                Instance = this;
            }
        }

        // Stores all the events
        // 涓€涓暟鎹瓧鍏革紝鐢ㄦ潵淇濆瓨浜嬩欢
        Dictionary<string, EventChannel> EventDictionary = new Dictionary<string, EventChannel>();

        // Function to allow an object to listen to a channel, and call a function(s) when a request to said channel is made
        // 璁╀竴涓墿浠舵敹娑堟伅鐨勫嚱鏁般€傚綋鐗╀欢鏀跺埌娑堟伅锛屼細璋冪敤鍑芥暟銆?
        public void Listen(string channelname, UnityAction<IEventRequestInfo> action)
        {
            if (!EventDictionary.ContainsKey(channelname))
            {
                EventDictionary.Add(channelname, new EventChannel());
            }
            EventChannel channel = EventDictionary[channelname];
            channel.AddListener(action);
        }

        // Allows an object to publish info to all listeners of a channel. Can send custom datatypes over.
        // 璁╀竴涓墿浠跺彂娑堟伅鐨勫嚱鏁般€備細鎶婅嚜璁㈣祫鏂欏彂缁欎竴涓閬撱€?
        public void Publish<T>(string channelname, object sender, T body)
        {
            EventChannel channel;
            if (EventDictionary.TryGetValue(channelname, out channel))
            {
                channel.Invoke(new EventRequestInfo<T>(channelname, sender, body));
            }
            else
            {
                Debug.LogError("Tried to publish an event to a non-existent event channel (Did you forget to register a channel, or was it a typo?)");
            }
        }

        // Allows an object to stop listening to a channel
        // 璁╀竴涓墿浠跺仠姝㈡敹娑堟伅鐨勫嚱鏁?
        public void Close(string channelname, UnityAction<IEventRequestInfo> action)
        {
            EventChannel channel;
            if (EventDictionary.TryGetValue(channelname, out channel))
            {
                channel.RemoveListener(action);
            }
        }
    }
}