using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPaintParticlesManager : MonoBehaviour
{
    public List<ParticleSystemRenderer> particleRenderers = new List<ParticleSystemRenderer>();

    int ColorID = Shader.PropertyToID("_Color");

    public void SetupPaintParticles(Color color)
    {
        foreach (var a in particleRenderers)
        {
            a.material.SetColor(ColorID, color);
        }
    }
}
