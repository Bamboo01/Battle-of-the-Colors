using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankecontrol : MonoBehaviour
{
    public float rospeed, speed;
     CharacterController controller;
   
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.RotateAround(transform.position, Vector3.up, horizontal * rospeed * Time.deltaTime);
       
        transform.Translate(Vector3.forward * speed * vertical * Time.deltaTime);

    }
}
