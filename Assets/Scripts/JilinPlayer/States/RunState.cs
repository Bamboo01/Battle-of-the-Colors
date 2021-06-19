using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RunState : FSMState
{
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float speed = 10f;
    public RunState(int stateID, MonoBehaviour mono, FSMManager manager) : base(stateID, mono, manager) { }
    public override void OnEnter()
    {
        mono.GetComponent<Animator>().SetBool("IsRun", true);
    }
    public override void OnUpdate()
    {

       
        if (PlayerControl.S.dir.magnitude>=0.1f)
       {
            
            float targetAngle = Mathf.Atan2(PlayerControl.S.dir.x, PlayerControl.S.dir.z) * Mathf.Rad2Deg + PlayerControl.S.cam.eulerAngles.y ;
            mono.transform.rotation = PlayerControl.S.cam.rotation;//Quaternion.Euler(0f, targetAngle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (PlayerControl.S.isStealth)
            {
                PlayerControl.S.controller.Move(moveDir.normalized * speed *3* Time.deltaTime);
            }
            if (PlayerControl.S.isStealth==false)
            {
                PlayerControl.S.controller.Move(moveDir.normalized * speed  * Time.deltaTime);
             
            }

            
        }
        else
        {
            fsmManager.ChangeState((int)PlayerState.idle);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerControl.S.isStealth == false)
        {
           
            mono.GetComponent<Animator>().SetBool("Stealth", true);
            //mono.transform.localScale += new Vector3(0, -1, 0);
            PlayerControl.S.controller.center = new Vector3(PlayerControl.S.controller.center.x, PlayerControl.S.controller.center.y + 1, PlayerControl.S.controller.center.z);
            PlayerControl.S.isStealth = true;
        }

        else if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerControl.S.isStealth == true)
        {
            mono.transform.position += new Vector3(0, 2, 0);
            PlayerControl.S.controller.center = new Vector3(PlayerControl.S.controller.center.x, PlayerControl.S.controller.center.y - 1, PlayerControl.S.controller.center.z);
            PlayerControl.S.isStealth = false;
            fsmManager.ChangeState((int)PlayerState.idle);
        }
    }

}
