using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategemSample : StrategemGameobjectBase
{
    NetworkCharacter owner;
    public override void onActivation(NetworkCharacter caller)
    {
        owner = caller;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
