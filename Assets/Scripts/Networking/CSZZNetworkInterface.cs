using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Bamboo.Utility;
using Bamboo.Events;
using Mirror;

// Handles calls from local objects to send towards the server
namespace CSZZGame.Networking
{
    public class CSZZNetworkInterface : Singleton<CSZZNetworkInterface>
    {
        NetworkPlayerScript currentClientPlayer;

        public void SendNetworkEvent<T>(string channel, T obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            currentClientPlayer.CmdRaiseEvent(channel, ms.ToArray());
        }

        public void SendNetworkEvent(string channel)
        {
            currentClientPlayer.CmdRaiseEvent(channel, null);
        }

        public static T DeserializeEventData<T>(byte[] data = null)
        {
            if (data == null)
            {
                return default(T);
            }
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(data, 0, data.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return (T)binForm.Deserialize(memStream);
        }

        public void SetupClient(NetworkPlayerScript p)
        {
            currentClientPlayer = p;
        }
    }
}