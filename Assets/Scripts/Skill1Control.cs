using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Control : MonoBehaviour
{
    public GameObject Skill;
    public float CD;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        Quaternion Q;
        Q = Quaternion.Euler(transform.rotation.x , transform.rotation.y+180, transform.rotation.z);

        GameObject bullet= Instantiate( Skill,transform.position+new Vector3(0,0f,0), Q);
        Destroy(gameObject);
        Destroy(bullet, CD);
    }
}
