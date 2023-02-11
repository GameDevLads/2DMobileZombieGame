using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CollectableV2")]
public class CollectableV2 : ScriptableObject
{
    public int collectableAmount = 0;

    public float collectableLifetime = 0f;
    public float blinkingDelayedStart = 0f;

    public Sprite defaultModel;
    public Sprite despawnModel;
    public void IncrementCollectable()
    {
        collectableAmount++;
    }
    public void DecrementCollectable()
    {
        collectableAmount--;
    }
}
