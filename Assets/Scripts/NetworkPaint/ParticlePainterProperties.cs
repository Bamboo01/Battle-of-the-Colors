using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParticlePainter", menuName = "new ParticlePainter")]
public class ParticlePainterProperties : ScriptableObject
{
    [Tooltip("Should be controlled by the Team, but if your script doesn't want to, define it here")]
    public Color color;
    [Range(0.01f, Mathf.Infinity)]
    public float minRadius = 0.8f;
    [Range(0.01f, Mathf.Infinity)]
    public float maxRadius = 0.8f;
    [Range(0.01f, Mathf.Infinity)]
    public float particleGravity = 1.0f;
    [Range(0.01f, Mathf.Infinity)]
    public float particleMinStartSpeed = 0.01f;
    [Range(0.01f, Mathf.Infinity)]
    public float particleMaxStartSpeed = 1.0f;
    [Range(0.01f, Mathf.Infinity)]
    public float particleColliderRadius = 0.15f;
}

[System.Serializable]
public class ParticlePainterPropertiesSerialized
{
    public SerializedVector4 color;
    public float radius;
    public float particleGravity;
    public float particleMinStartSpeed;
    public float particleMaxStartSpeed;
    public float particleColliderRadius;

    public ParticlePainterPropertiesSerialized(ParticlePainterProperties props)
    {
        color = new SerializedVector4(props.color);
        radius = Random.Range(props.minRadius, props.maxRadius);
        particleGravity = props.particleGravity;
        particleMinStartSpeed = props.particleMinStartSpeed;
        particleMaxStartSpeed = props.particleMaxStartSpeed;
        particleColliderRadius = props.particleColliderRadius;
    }

    public ParticlePainterPropertiesSerialized(ParticlePainterProperties props, Color c)
    {
        color = new SerializedVector4(c);
        radius = Random.Range(props.minRadius, props.maxRadius);
        particleGravity = props.particleGravity;
        particleMinStartSpeed = props.particleMinStartSpeed;
        particleMaxStartSpeed = props.particleMaxStartSpeed;
        particleColliderRadius = props.particleColliderRadius;
    }
}