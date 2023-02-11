using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/String Variable")]
public class StringVariableSO : ScriptableObject
{
  public string Value;
  public bool IsNull;
}
