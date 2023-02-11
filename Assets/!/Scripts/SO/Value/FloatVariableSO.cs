using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FloatVariable")]
public class FloatVariableSO : ScriptableObject
{
    public float Value;
    public bool IsNull;
}
