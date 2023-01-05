using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectable")]
public class Collectable : ScriptableObject
{
    public float collectableLifetime = 0f;
    public float blinkingDelayedStart = 0f;
    public float blinkingInterval = 0f;
}
