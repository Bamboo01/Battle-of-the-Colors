using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class climbState : FSMState
{
    public climbState(int stateID, MonoBehaviour mono, FSMManager manager) : base(stateID, mono, manager) { }
    public static climbState S;
    private void Awake()
    {

        S = this;

    }

    Vector3 headPos;
    RaycastHit hitInfo;
    Vector2 input;


    public Transform climbHelper;



    public override void OnEnter()
    {
        mono.GetComponent<Animator>().SetBool("IsClimbing", true);
    }


    public override void OnUpdate()
    {
        Debug.Log("climb");
        if (PlayerControl.S.onWall == false)
        {
            SetBodyPositionToWall();
        }
        else if (PlayerControl.S.onWall)
        {
            FixBodyPos();
            MoveHandler();
        }

    }

    private void SetBodyPositionToWall()
    {
        float a = Vector3.Distance(mono.transform.position, PlayerControl.S.targetPos);

        if (a < 0.1f)
        {

            PlayerControl.S.onWall = true;
            mono.transform.position = PlayerControl.S.targetPos;
            return;
        }
        Vector3 lerpTargetPos = Vector3.MoveTowards(mono.transform.position, PlayerControl.S.targetPos, 0.2f);
        mono.transform.position = lerpTargetPos;
    }

    public void FixBodyPos()
    {
        Vector3 localClimbHelperPos = mono.transform.InverseTransformPoint(climbHelper.position);
        Vector3 localHeadPos = new Vector3(localClimbHelperPos.x, localClimbHelperPos.y, 0);
        headPos = mono.transform.TransformPoint(localHeadPos);
        Debug.DrawRay(headPos, mono.transform.forward * 1f, Color.red); ;
        if (Physics.SphereCast(headPos, 0.1f, mono.transform.forward, out hitInfo, 1f))
        {
            Vector3 tempVector = mono.transform.position - climbHelper.position;
            if (Vector3.Distance(mono.transform.position, hitInfo.point + tempVector) > 0.05f)
            {
                mono.transform.position = hitInfo.point + tempVector;
            }
            mono.transform.position = hitInfo.point + tempVector;
        }
    }
    public void MoveHandler()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        if (input.magnitude > 0.5f)
        {
            mono.transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        }
    }
    /* private void OnDrawGizmos()
     {
         Gizmos.color = Color.red;
         Gizmos.DrawSphere(headPos, 0.1f);
         Gizmos.color = Color.blue;
         Gizmos.DrawSphere(hitInfo.point, 0.1f);
         Debug.Log(5);
     }*/

}
