using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Vector3Variable")]
public class Vector3VariableSO : ScriptableObject
{
    public Vector3 Value;
    public bool IsNull;
}
