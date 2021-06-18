using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager S;
    private void Awake()
    {
        
            S = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //if (!GameManager.isSkill)
        //return;
        Type();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ArrowGolder.S.isFinish|| ArrowGolder.S.isFinish1|| ArrowGolder.S.isFinish2)
            {
                GameManager.S.FinishWave();
            }                    
            else
            {
                GameManager.S.FailWave();
            }
        }
       
    }
    void Type()
    {
        ArrowGolder golder = GetComponent<ArrowGolder>();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            golder.TypeArrow(KeyCode.UpArrow);
            golder.TypeArrow1(KeyCode.UpArrow);
            golder.TypeArrow2(KeyCode.UpArrow);
        }
           
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            golder.TypeArrow(KeyCode.DownArrow);
            golder.TypeArrow1(KeyCode.DownArrow);
            golder.TypeArrow2(KeyCode.DownArrow);
        }
            
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            golder.TypeArrow(KeyCode.LeftArrow);
            golder.TypeArrow1(KeyCode.LeftArrow);
            golder.TypeArrow2(KeyCode.LeftArrow);
        }
           
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            golder.TypeArrow(KeyCode.RightArrow);
            golder.TypeArrow1(KeyCode.RightArrow);
            golder.TypeArrow2(KeyCode.RightArrow);
        }
            
    }
}
