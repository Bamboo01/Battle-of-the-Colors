using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGolder : MonoBehaviour
{
    public static ArrowGolder S;
    private void Awake()
    {
        isFrist = isFrist1 = isFrist2 = false;
        S = this;

    }
    public GameObject arrowPrefab;
    public Transform[] arrowsHolder;
    public  bool isFinish;
    private Queue<Arrow> arrows= new Queue<Arrow>();
    Arrow currentArrow ;
    public bool isFrist = true;
    private Queue<Arrow> arrows1 = new Queue<Arrow>();
    Arrow currentArrow1;   
    public bool isFrist1 = true;
    public  bool isFinish1;
    private Queue<Arrow> arrows2 = new Queue<Arrow>();
    Arrow currentArrow2;
    public bool isFrist2 = true;
    public  bool isFinish2;
    public bool SkillOK;
    //生成
   
    public void CreateWave(int Length)
    {

        isFrist  = true;
         //清空队列
         arrows = new Queue<Arrow>();
        isFinish = false;
        for (int i = 0; i < Length; i++)
        {
            Arrow newarrow = Instantiate(arrowPrefab, arrowsHolder[0]).GetComponent<Arrow>();
            int randomDir = Random.Range(0, 3);
            newarrow.SetUp(randomDir);
            arrows.Enqueue(newarrow);            
        }
        currentArrow = arrows.Dequeue();    

    }
    public void CreateWave1(int Length)
    {
        isFrist1 = true;
       arrows1 = new Queue<Arrow>();
        isFinish1 = false;
        for (int i = 0; i < Length; i++)
        {
            Arrow newarrow = Instantiate(arrowPrefab, arrowsHolder[1]).GetComponent<Arrow>();
            int randomDir = Random.Range(0, 3);
            newarrow.SetUp(randomDir);
            arrows1.Enqueue(newarrow);
        }
        currentArrow1 = arrows1.Dequeue();
    }

    public void CreateWave2(int Length)
    {
        isFrist2 = true;
       arrows2 = new Queue<Arrow>();
        isFinish2 = false;
        for (int i = 0; i < Length; i++)
        {
            Arrow newarrow = Instantiate(arrowPrefab, arrowsHolder[2]).GetComponent<Arrow>();
            int randomDir = Random.Range(0, 3);
            newarrow.SetUp(randomDir);
            arrows2.Enqueue(newarrow);
        }
        currentArrow2 = arrows2.Dequeue();
    }
    public void TypeArrow(KeyCode inputKey)
    {

        if (currentArrow == null)
            return;
        if (ConvertKeyCodeToInt(inputKey) == currentArrow.arrowDir)
        {
           
            currentArrow.SetFinish();
            //rearrows.Enqueue(currentArrow);
            if (arrows.Count > 0)
            {
                currentArrow = arrows.Dequeue();                
            }
            else if(arrows.Count==0)
            {
                boolcontrol.SkillNO = 1;
                isFinish = true;
                
                
            }
        }

        else
            isFrist = false;
    }
    public void TypeArrow1(KeyCode inputKey)
    {
        if (currentArrow1 == null)
        return;
        if (ConvertKeyCodeToInt(inputKey) == currentArrow1.arrowDir)
        {

            currentArrow1.SetFinish();
            //rearrows.Enqueue(currentArrow);
            if (arrows1.Count > 0)
            {
                currentArrow1 = arrows1.Dequeue();

            }
            else if (arrows1.Count == 0)
            {
                boolcontrol.SkillNO = 2;
                isFinish1 = true;


            }
        }
        else
            isFrist1 = false;
    }
    public void TypeArrow2(KeyCode inputKey)
    {

        if (currentArrow2 == null)
        return;
        if (ConvertKeyCodeToInt(inputKey) == currentArrow2.arrowDir)
        {

            currentArrow2.SetFinish();
            //rearrows.Enqueue(currentArrow);
            if (arrows2.Count > 0)
            {
                currentArrow2 = arrows2.Dequeue();

            }
            else if (arrows2.Count == 0)
            {
                boolcontrol.SkillNO = 3;
                isFinish2 = true;

            }
        }
        else
            isFrist2 = false;
        if (!isFrist && !isFrist1 && !isFrist2)
            GameManager.S.FailWave();
    }
   //清理
    public  void ClearWave()
    {
        //SkillCD1.CD1 = SkillCD1.CD2 = SkillCD1.CD3 = 0f;
        SkillCD1.a= SkillCD1.b= SkillCD1.c = false;
        arrows = new Queue<Arrow>();
        arrows1 = new Queue<Arrow>();
        arrows2 = new Queue<Arrow>();
        // isFinish = false;
        foreach (Transform arrow in arrowsHolder[0])
         {
             Destroy(arrow.gameObject);
         }
        foreach (Transform arrow1 in arrowsHolder[1])
        {
            Destroy(arrow1.gameObject);
        }
        foreach (Transform arrow2 in arrowsHolder[2])
        {
            Destroy(arrow2.gameObject);
        }
    }
    //将输入按键转换成数值方便判断输入
    int ConvertKeyCodeToInt(KeyCode key)
    {
        int result = 0;
        switch (key)
        {
            case KeyCode.UpArrow:
                {
                    result = 0;
                    break;
                }
            case KeyCode.DownArrow:
                {
                    result = 1;
                    break;
                }
            case KeyCode.LeftArrow:
                {
                    result = 2;
                    break;
                }
            case KeyCode.RightArrow:
                {
                    result = 3;
                    break;
                }

        }
        return result;
    }

}
