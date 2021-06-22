using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializedVector4
{
    float x;
    float y;
    float z;
    float w;

    public SerializedVector4(Vector2 v)
    {
        x = v.x;
        y = v.y;
        z = 0;
        w = 0;
    }

    public SerializedVector4(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = 0;
    }

    public SerializedVector4(Vector4 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = v.w;
    }

    public SerializedVector4(Color v)
    {
        x = v.r;
        y = v.g;
        z = v.b;
        w = v.a;
    }

    public static explicit operator Vector2(SerializedVector4 ssvec4) => new Vector2(ssvec4.x, ssvec4.y);
    public static explicit operator Vector3(SerializedVector4 ssvec4) => new Vector3(ssvec4.x, ssvec4.y, ssvec4.z);
    public static explicit operator Vector4(SerializedVector4 ssvec4) => new Vector4(ssvec4.x, ssvec4.y, ssvec4.z, ssvec4.w);
    public static explicit operator Color(SerializedVector4 ssvec4) => new Color(ssvec4.x, ssvec4.y, ssvec4.z, ssvec4.w);


    public static SerializedVector4[] vec2ArrayToSerializedVec4 (Vector2[] vec2Array)
    {
        return Array.ConvertAll(vec2Array, s => new SerializedVector4(s));
    }

    public static SerializedVector4[] vec3ArrayToSerializedVec4(Vector3[] vec2Array)
    {
        return Array.ConvertAll(vec2Array, s => new SerializedVector4(s));
    }

    public static SerializedVector4[] vec4ArrayToSerializedVec4(Vector4[] vec2Array)
    {
        return Array.ConvertAll(vec2Array, s => new SerializedVector4(s));
    }

    public static SerializedVector4[] vec4ArrayToSerializedVec4(Color[] vec2Array)
    {
        return Array.ConvertAll(vec2Array, s => new SerializedVector4(s));
    }
}
