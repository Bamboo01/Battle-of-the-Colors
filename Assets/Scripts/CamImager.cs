using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamImager : MonoBehaviour
{
    public GameObject imager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        if (boolcontrol.cam)
        {
            imager.SetActive(boolcontrol.cam);
            Invoke("Shutdown", 15);
            boolcontrol.cam = false;
        }
        
    }
    void Shutdown()
    {
        imager.SetActive(false);
    }
}
