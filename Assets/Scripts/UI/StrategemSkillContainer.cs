using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategemSkillContainer : MonoBehaviour
{
    [SerializeField] Image fillBackground;
    [SerializeField] Image fillImage;
    [SerializeField] public GameObject arrowContainer;
    bool isArrowShown = false;

    public void setupContainer(Sprite strategemSprite)
    {
        fillBackground.sprite = strategemSprite;
        fillImage.sprite = strategemSprite;
        HideArrows(true);
    }

    public void updateFill(float curr, float max)
    {
        fillImage.fillAmount = Mathf.Min((max - curr) / max, 1.0f);
    }

    public void HideArrows(bool a)
    {
        isArrowShown = a;
        arrowContainer.gameObject.SetActive(a);
    }
}
