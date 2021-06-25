using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strategem Properties", menuName = "Strategem Properties")]
public class StratagemProperties : ScriptableObject
{
    Sprite stratagemSprite;
    string stragagemName;
    int executionLength;    
    float cooldownTime;
    StratagemGameobject strategem;
}
