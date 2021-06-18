using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform PlayerPos;
    public GameObject SkillPre,Skill;
    public static GameManager S;
    private void Awake()
    {
        
            S = this;
       
    }
    public int currentLevel=4;
    private void Start()
    {
                
    }
   
    void NextWave()
    {
      
        ArrowGolder.S.CreateWave(currentLevel);

    }
    
    public void FinishWave()
    {

        ArrowGolder.S.ClearWave();
        ArrowGolder.S.isFrist =ArrowGolder.S.isFrist1= ArrowGolder.S.isFrist2 = false;
        Create();
        if (ArrowGolder.S.isFinish|| ArrowGolder.S.isFinish1|| ArrowGolder.S.isFinish2)
        {
            GetComponent<Animator>().SetTrigger("Skill");
            Instantiate(SkillPre, PlayerPos.transform.position, PlayerPos.transform.rotation);
            Skill.SetActive(false);
        }
      
               
    }
   
    public void FailWave()
    {
        ArrowGolder.S.ClearWave();
        //ArrowGolder.S.ClearWave();
    }
    void Create()
    {
        if (boolcontrol.SkillNO == 1)
        {
            SkillCD1.CD1 = 0f;
        }
        if (boolcontrol.SkillNO == 2)
        {
            SkillCD1.CD2 = 0f;
        }
        if (boolcontrol.SkillNO == 3)
        {
            SkillCD1.CD3 = 0f;
        }
    }
}
