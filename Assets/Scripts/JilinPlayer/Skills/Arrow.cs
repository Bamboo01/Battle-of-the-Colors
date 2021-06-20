using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public Sprite[] arrowSprites;
    Image image;
    [HideInInspector]
    public int arrowDir;
    private Color finishColor,startColor;
    private void Awake()
    {
        image = GetComponent<Image>();
        startColor = new Color(1, 1, 1, 1);
        finishColor = new Color(0, 1, 0, 1);

    }
    private void OnEnable()
    {
        
    }

    public void SetUp(int dir)
    {
        arrowDir = dir;
         image.sprite=arrowSprites[dir];
        image.SetNativeSize();
    }
    
    public void SetFinish()
    {
        image.color = finishColor;
       
    }
    public  void SetStart()
    {
        image.color = startColor;   
    }
}
