using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Bamboo.Utility;
using Bamboo.Events;
using CSZZGame.Refactor;
using CSZZGame.Networking;

public class NetworkPainterManager : Singleton<NetworkPainterManager>
{
    private int brushcolorID = Shader.PropertyToID("cBrushColor");
    private int brushpositionID = Shader.PropertyToID("cBrushPosition");
    private int brushradiusID = Shader.PropertyToID("cBrushRadius");
    private int modelmatrixID = Shader.PropertyToID("ModelMatrix");
    private int rawcolormasktexID = Shader.PropertyToID("cRawColorMaskTex");
    private int unwrappeduvtexID = Shader.PropertyToID("cUnwrappedUVTex");

    // Painter Shader IDs
    private int unwrapperScaleMatrixID = Shader.PropertyToID("_ScaleMatrix");
    private int masktexID = Shader.PropertyToID("cMaskTex");
    private int inversescalingmatrixID = Shader.PropertyToID("InverseActualScaleMatrix");

    // Materials
    private Material unwrapperMaterial;

    //Shaders
    [SerializeField] Shader TextureUnwrapper;
    [SerializeField] ComputeShader TexturePainter;

    // Paint Calculator
    [SerializeField] PaintCalculator paintCalculator;

    //Command Buffer
    CommandBuffer commandbuffer;

    // Paintable IDs (Should be the same given that their loading the same scene)
    int currentPaintableID = 0;
    Dictionary<int, Paintable> idToPaintable = new Dictionary<int, Paintable>();
    Dictionary<Paintable, int> paintableToID = new Dictionary<Paintable, int>();

    public void Awake()
    {
        _persistent = false;
        unwrapperMaterial = new Material(TextureUnwrapper);
        commandbuffer = new CommandBuffer();
    }

    public void Start()
    {
        EventManager.Instance.Listen(EventChannels.OnPaintPaintableEvent, OnPaintPaintableEvent);
        Color[] colors = ServerCharacterData.getAllTeamColors();
        int i = 0;
        foreach (var a in colors)
        {
            paintCalculator.teamColors[i] = a;
            paintCalculator.colorCounterList.Add(0);
            i++;
        }
    }

    public void OnPaintPaintableEvent(IEventRequestInfo eventRequestInfo)
    {
        EventRequestInfo<byte[]> info = (EventRequestInfo<byte[]>)eventRequestInfo;
        PaintParticlesNetworkData data = CSZZNetworkInterface.DeserializeEventData<PaintParticlesNetworkData>(info.body);
        foreach (var pos in data.collisionPositions)
        {
            Paint(idToPaintable[data.paintableID], (Color)data.actualColor, (Vector3)pos, data.painterProperties.radius * data.paintScale);
        }
    }

    public void SetupPaintable(Paintable paintable)
    {
        unwrapperMaterial.SetMatrix(unwrapperScaleMatrixID, paintable.scalingMatrix);
        commandbuffer.SetRenderTarget(paintable.uvposTexture);
        commandbuffer.DrawRenderer(paintable.renderer, unwrapperMaterial);
        Graphics.ExecuteCommandBuffer(commandbuffer);
        commandbuffer.Clear();

        idToPaintable.Add(currentPaintableID, paintable);
        paintableToID.Add(paintable, currentPaintableID);
        currentPaintableID++;

        paintCalculator.onPaintableAdded(paintable);
    }

    public void Paint(Paintable paintable, Color color, Vector3 position, float radius)
    {
        TexturePainter.SetTexture(0, unwrappeduvtexID, paintable.uvposTexture);
        TexturePainter.SetTexture(0, masktexID, paintable.maskTexture);
        TexturePainter.SetTexture(0, rawcolormasktexID, paintable.rawmaskcolorTexture);

        TexturePainter.SetVector(brushcolorID, color);
        TexturePainter.SetVector(brushpositionID, position);
        TexturePainter.SetFloat(brushradiusID, radius);

        Matrix4x4 TRS = paintable.transform.localToWorldMatrix;
        TRS *= paintable.inversescalingMatrix;
        TexturePainter.SetMatrix(inversescalingmatrixID, paintable.inversescalingMatrix);
        TexturePainter.SetMatrix(modelmatrixID, TRS);

        TexturePainter.Dispatch(0, (int)paintable.textureSize / 8, (int)paintable.textureSize / 8, 1);
    }

    public int GetPaintableID(Paintable paintable)
    {
        return paintableToID[paintable];
    }
}
