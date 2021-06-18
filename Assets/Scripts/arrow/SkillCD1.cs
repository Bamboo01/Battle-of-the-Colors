using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCD1 : MonoBehaviour
{
    public Slider Skill1,Skill2,Skill3;

    public static float CD1 , CD2 , CD3 ;
    public static bool a,b,c;
    
    // Start is called before the first frame update
    void Start()
    {
        a = b = c = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CD1 <= 3)
        {
            CD1 += Time.deltaTime;
            Skill1.value = CD1 / 3f;
        }          
        if (Skill1.value >= 1 && a==false)
        {
            ArrowGolder.S.CreateWave(GameManager.S.currentLevel);
            a =true;          
        }

        if (CD2 <= 5)
        {
            CD2 += Time.deltaTime;
            Skill2.value = CD2 / 5f;
        }
        if (Skill2.value >= 1 && b == false)
        {

            ArrowGolder.S.CreateWave1(GameManager.S.currentLevel);
            b = true;
        }

        if (CD3 <= 10)
        {
           
            CD3 += Time.deltaTime;
            Skill3.value = CD3 / 10f;
        }
        if (Skill3.value >= 1 && c == false)
        {
            ArrowGolder.S.CreateWave2(GameManager.S.currentLevel);
            c = true;
        }
    }
    
}
