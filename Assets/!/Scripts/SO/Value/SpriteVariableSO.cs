using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Sprite Variable")]
public class SpriteVariableSO : ScriptableObject
{
  public Sprite Value;
  public bool IsNull;
}
