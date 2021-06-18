using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSMState
{
   public IdleState(int stateID,MonoBehaviour mono,FSMManager manager) : base(stateID, mono, manager) { }
    public override void OnEnter()
    {
        mono.GetComponent<Animator>().SetBool("IsRun", false);
    }
    public override void OnUpdate()
    {

        //¼àÌýÅÜ²½×´Ì¬ÇÐ»»
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        if (dir != Vector3.zero)
        {
            fsmManager.ChangeState((int)PlayerState.run);
        }

        if (PlayerControl.S.isClimbing)
        {
            fsmManager.ChangeState((int)PlayerState.climb);
        }
        
      
    }
}
