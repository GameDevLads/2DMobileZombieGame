using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectable")]
public class Collectable : ScriptableObject
{
    public int collectableAmount = 0;
    public float collectableLifetime = 0f;
    public float blinkingDelayedStart = 0f;
    public float blinkingInterval = 0f;

    public void IncrementCollectable()
    {
        collectableAmount++;
    }
    public void DecrementCollectable()
    {
        collectableAmount--;
    }

}
