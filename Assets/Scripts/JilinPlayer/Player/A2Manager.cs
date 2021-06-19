using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A2Manager : MonoBehaviour
{
    public Transform player,A2Point;
    public GameObject A2;
    private bool isappea=false,isOK=true;
    public float CD = 0f;
    public GameObject Skill;
    public Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Point = new Vector3(A2Point.position.x,2, A2Point.position.z);

        if (Input.GetKeyDown(KeyCode.V) && isOK)
        {
            CD = 0f;          
            isOK = false;
           GameObject go= Instantiate(A2, Point , player.transform.rotation);
            Destroy(go, 3f);
        }
        if (isOK == false)
        {
            CD += Time.deltaTime;
        }
        if (CD >= 6f)
            isOK = true;
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Skill.SetActive(true);
            Ani.SetTrigger("turnSkill");
        }
    }
}
