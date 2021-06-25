using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBrushParticlesManager : MonoBehaviour
{
    public List<NetworkParticlePainter> painters = new List<NetworkParticlePainter>();
    public void SetupServerParticles(ParticlePainterProperties props, Color color, float scale)
    {
        foreach (var a in painters)
        {
            Vector3 _scale = a.transform.localScale;
            _scale *= scale;
            a.transform.localScale = _scale;
            a.particlePainterProperties = props;
            a.SetupPainter(color, scale);
            a.gameObject.SetActive(true);
        }
    }
}
