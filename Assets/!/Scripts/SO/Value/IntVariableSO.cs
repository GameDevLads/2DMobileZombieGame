using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/IntVariable")]
public class IntVariableSO : ScriptableObject
{
    public int Value;
    public bool IsNull;
}
