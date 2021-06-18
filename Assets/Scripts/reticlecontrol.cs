using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticlecontrol : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(-TankCam.my * Vector3.up * Time.deltaTime);
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2-3*TankCam.my, 0);
    }
}
