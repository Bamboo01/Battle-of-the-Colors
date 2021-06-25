using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPaintParticlesManager : MonoBehaviour
{
    public List<ParticleSystemRenderer> particleRenderers = new List<ParticleSystemRenderer>();

    int ColorID = Shader.PropertyToID("_Color");

    public void SetupPaintParticles(Color color, float scale)
    {
        foreach (var a in particleRenderers)
        {
            Vector3 _scale = a.transform.localScale;
            _scale *= scale;
            a.transform.localScale = _scale;
            a.material.SetColor(ColorID, color);
        }
    }
}
