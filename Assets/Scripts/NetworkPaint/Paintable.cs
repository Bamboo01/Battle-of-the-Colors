using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour
{
    public enum TextureSize
    {
        _2056x2056 = 2056,
        _1024x1024 = 1024,
        _512x512 = 512,
        _256x256 = 256,
        _128x128 = 128
    };

    [Header("Texture Size")]
    public TextureSize textureSize = TextureSize._1024x1024;
    [Tooltip("Normalized scale (Scaled to 1 unit) - (Enable gizmos to view, adjust this until the wiremesh fits the box)")]
    [SerializeField] public Vector3 normalizedScale = new Vector3(1, 1, 1);
    [SerializeField] public Mesh sceneMesh;
    [SerializeField] public int paintableScore = 1;
    [Header("Raycast Threshold")]
    float maxDistance = 0.1f;

    [HideInInspector] public Matrix4x4 scalingMatrix;
    [HideInInspector] public Matrix4x4 inversescalingMatrix;
    [HideInInspector] public Renderer renderer;


    [HideInInspector] public RenderTexture uvposTexture;
    [HideInInspector] public RenderTexture maskTexture;
    [HideInInspector] public RenderTexture rawmaskcolorTexture;
    [HideInInspector] public List<RenderTexture> rawcolorTextureMipMap = new List<RenderTexture>();


    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Gizmos.matrix = Matrix4x4.identity;
        if (sceneMesh)
        {
            Gizmos.DrawWireMesh(sceneMesh, transform.position, transform.rotation, normalizedScale);
        }
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, 1));
#endif
    }

    private void Start()
    {
        // Setup the renderer
        renderer = GetComponent<Renderer>();

        // Setup the unwrapped uv texture
        uvposTexture = new RenderTexture((int)textureSize, (int)textureSize, 0, RenderTextureFormat.ARGBHalf);
        uvposTexture.filterMode = FilterMode.Bilinear;
        uvposTexture.enableRandomWrite = true;
        uvposTexture.Create();

        // Setup mask texture
        maskTexture = new RenderTexture((int)textureSize, (int)textureSize, 0, RenderTextureFormat.ARGB32);
        maskTexture.filterMode = FilterMode.Bilinear;
        maskTexture.enableRandomWrite = true;
        maskTexture.Create();

        // Setup raw color texture to sample for movements
        rawmaskcolorTexture = new RenderTexture((int)textureSize, (int)textureSize, 0, RenderTextureFormat.ARGB32);
        rawmaskcolorTexture.filterMode = FilterMode.Bilinear;
        rawmaskcolorTexture.enableRandomWrite = true;
        rawmaskcolorTexture.Create();

        scalingMatrix = Matrix4x4.Scale(normalizedScale);
        inversescalingMatrix = scalingMatrix.inverse;

        NetworkPainterManager.Instance.SetupPaintable(this);

        // Debug
        renderer.material.SetTexture("_MaskTexture", maskTexture);

        //PaintCalculator.Instance.AddPaintable(this);
    }
}
