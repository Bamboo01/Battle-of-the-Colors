using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullut : MonoBehaviour
{
    public float speed;
    bool istrigger;
    float destryCD;
    // Start is called before the first frame update
    private void Start()
    {
        istrigger = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(istrigger)
            Destroy(this.gameObject);
        
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (!istrigger)
        {
            destryCD += Time.deltaTime;
        }
        if (destryCD >= 10)
        {
            istrigger = true;
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        istrigger = true;
       
    }
    
}
