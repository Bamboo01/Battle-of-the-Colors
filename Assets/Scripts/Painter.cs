using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painter : MonoBehaviour
{
    [SerializeField] SimplePaintBrush brush;
    [SerializeField] Texture2D pixelSampler;

    [SerializeField] ComputeShader debug;
    public Image colorimage;
    Sprite blankSprite;

    private void OnGUI()
    {
        GUI.color = Color.white;
        if (pixelSampler)
        {
            GUI.Box(new Rect(0, 400, 400, 400), pixelSampler);
            GUI.Label(new Rect(0, 400, 400, 400), "pixelSampler");
        }
    }

    private void Awake()
    {
        pixelSampler = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        blankSprite = Sprite.Create(pixelSampler, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit raycastHit;
        //    if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        //    {
        //        brush.position = raycastHit.point;
        //        Paintable paintable = raycastHit.transform.GetComponent<Paintable>();
        //        if (paintable)
        //        {
        //            PaintManager.instance.Paint(paintable, ref brush);
        //        }
        //    }
        //}

        //if (Input.GetMouseButton(1))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit raycastHit;
        //    if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        //    {
        //        brush.position = raycastHit.point;
        //        Paintable paintable = raycastHit.transform.GetComponent<Paintable>();
        //        if (paintable)
        //        {
        //            int x = (int)((float)paintable.textureSize * raycastHit.textureCoord.x);
        //            int y = (int)((float)paintable.textureSize * raycastHit.textureCoord.y);

        //            RenderTexture.active = paintable.rawmaskcolorTexture;
        //            pixelSampler.ReadPixels(new Rect(x, (int)paintable.textureSize - y, 1, 1), 0, 0, false);
        //            pixelSampler.Apply();

        //            Color color = pixelSampler.GetPixel(0, 0);
        //            Debug.Log(color);

        //            RenderTexture.active = null;
                    
        //            colorimage.sprite = blankSprite;
        //        }
        //    }
        //}
    }
}
