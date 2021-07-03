using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankecontrol : MonoBehaviour
{
    public float rospeed, speed;
     CharacterController controller;
    public Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        controller= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        Vector3 moveDir = transform.forward * vertical * Time.deltaTime*speed;
        transform.RotateAround(transform.position, Vector3.up, horizontal * rospeed * Time.deltaTime);
        controller.Move(moveDir);
        controller.SimpleMove(Physics.gravity);
        if (moveDir != Vector3.zero)
        {
            Ani.SetBool("isRun", true);
        }else if (moveDir == Vector3.zero)
        {
            Ani.SetBool("isRun", false);
        }
    }
}
