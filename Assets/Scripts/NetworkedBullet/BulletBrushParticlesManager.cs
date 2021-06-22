using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBrushParticlesManager : MonoBehaviour
{
    public List<NetworkParticlePainter> painters = new List<NetworkParticlePainter>();
    public void SetupServerParticles(ParticlePainterProperties props)
    {
        foreach (var a in painters)
        {
            a.particlePainterProperties = props;
            a.SetupPainter();
            a.gameObject.SetActive(true);
        }
    }
}
