using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public IntVariableSO intVariableSO;

    public int coinAmount;
    private void Update()
    {
        coinAmount = intVariableSO.Value;
    }
}
