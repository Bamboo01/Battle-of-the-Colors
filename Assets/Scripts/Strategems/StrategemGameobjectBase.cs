using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StrategemGameobjectBase : MonoBehaviour
{
    // This is a server object. On spawning, will execute a function on the server thaat may choose to invoke
    // an event to all clients, or change variables, or cause something to happen in the scene. All in all, this should
    // ONLY BE RUN ON SERVER SIDE!!! No matter what!

    // Behaviours such as when to activate, what to do on startup should be defined in the monobehaviour functions
    // like start, update of the child strategem
    public abstract void onActivation(NetworkCharacter caller);
}
