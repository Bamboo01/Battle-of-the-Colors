using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boolcontrol : MonoBehaviour
{
    public GameObject Skill1, Skill2;
    float timeval = 0.3f,TempTime;
    int number;
    public static float SkillNO=1;
    public static bool isFrist;
    public static bool cam;
    // Start is called before the first frame update
    void Start()
    {
        isFrist = true;
        TempTime = timeval;
        number = 0;
        cam = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(TempTime);
        transform.Translate(Vector3.left * 10 * Time.deltaTime);      
        Invoke("switchSkill", 1.9f);
        
    }
    public void switchSkill()
    {
        TempTime -= Time.deltaTime;
        Vector3 dir = new Vector3(transform.position.x,30f, transform.position.z);
        Vector3 zero = new Vector3(transform.position.x, 0f, transform.position.z);
        if (isFrist == true)
        {                       
            if (SkillNO == 1)
            {
                

                 if (TempTime <= 0)
                 {
                     Instantiate(Skill1, dir+new Vector3(Random.Range(-3,3),0, Random.Range(-3, 3)), Skill1.transform.rotation);
                     TempTime = timeval;
                     number++;

                 }
                 if (number >= 6)
                {
                    Destroy(gameObject);
                    isFrist = false;
                }
                    
                
   
            }
            if (SkillNO == 2)
            {

                if (TempTime <= 0)
                {
                    Instantiate(Skill2, dir + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)), Skill2.transform.rotation);
                    TempTime = timeval;
                    number++;
                }
                if (number >= 3)
                {
                    Destroy(gameObject);
                    isFrist = false;
                }
                   
              
                
            }
            if (SkillNO == 3)
            {

                cam = true;

                isFrist = false;
            }
        }
        

    }
}
