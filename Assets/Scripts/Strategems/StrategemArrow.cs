using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StrategemArrow : MonoBehaviour
{
    [SerializeField] Sprite[] arrowSprites;
    [HideInInspector] public int arrowDir;
    private Color initialColor, finishedColor;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        initialColor = new Color(1, 1, 1, 1);
        finishedColor = new Color(0, 1, 0, 1);

    }

    public void SetupArrow(int dir)
    {
        arrowDir = dir;
        image.sprite = arrowSprites[dir];
        image.SetNativeSize();
    }

    public void SetFinish()
    {
        image.color = finishedColor;

    }
    public void SetStart()
    {
        image.color = initialColor;
    }
}
