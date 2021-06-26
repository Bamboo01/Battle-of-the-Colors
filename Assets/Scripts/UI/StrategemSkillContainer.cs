using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategemSkillContainer : MonoBehaviour
{
    [SerializeField] Image fillBackground;
    [SerializeField] Image fillImage;
    [SerializeField] public GameObject arrowContainer;
    public int commandLength = -1;
    private bool isArrowShown = false;
    private int currentArrowIndex;

    public void setupContainer(Sprite strategemSprite, int length)
    {
        fillBackground.sprite = strategemSprite;
        fillImage.sprite = strategemSprite;
        commandLength = length;
        showArrows(true);
    }

    public void updateFill(float curr, float max)
    {
        fillImage.fillAmount = Mathf.Min((max - curr) / max, 1.0f);
    }

    public void showArrows(bool a)
    {
        isArrowShown = a;
        arrowContainer.gameObject.SetActive(a);
    }

    public void ClearArrows()
    {
        foreach (Transform transform in arrowContainer.transform)
        {
            Destroy(transform.gameObject);
        }
        currentArrowIndex = 0;
    }
    
    public void SetArrowFinish()
    {
        arrowContainer.transform.GetChild(currentArrowIndex).GetComponent<StrategemArrow>().SetFinish();
        currentArrowIndex++;
    }
}
