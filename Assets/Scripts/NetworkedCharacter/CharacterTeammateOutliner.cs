using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTeammateOutliner : MonoBehaviour
{
    [SerializeField] Material outlineMat;

    Renderer outlineRenderer;
    GameObject outlineGameObject;
    bool isNotSupposedToOutline = false;

    public void CreateOutline(Color outlineColor)
    {
        if (isNotSupposedToOutline)
        {
            return;
        }

        outlineGameObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        Renderer rend = outlineGameObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetVector("_ParentObjectScale", transform.localScale);
        rend.material.SetFloat("_ScaleModifier", 1.05f);
        rend.material.SetColor("_OutlineColor", outlineColor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    [ContextMenu("Create an outline GameObject")]
    public void CreateOutline()
    {
        outlineGameObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        Renderer rend = outlineGameObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetVector("_ParentObjectScale", transform.localScale);
        rend.material.SetFloat("_ScaleModifier", 1.05f);
        rend.material.SetColor("_OutlineColor", Color.red);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void HideOutline()
    {
        if (outlineGameObject != null)
        {
            outlineGameObject.SetActive(false);
        }
        else
        {
            isNotSupposedToOutline = true;
        }
    }
}
