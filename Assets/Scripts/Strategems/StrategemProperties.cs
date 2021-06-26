using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strategem Properties", menuName = "Strategem Properties")]
public class StrategemProperties : ScriptableObject
{
    public int strategemID;
    public Sprite strategemSprite;
    public string strategemName;
    public int executionLength;
    public float cooldownTime;
    public GameObject strategemPrefab;
}
