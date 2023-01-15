using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public IntVariableSO coinAmountSO;

    public int coinAmount;
    private void Update()
    {
        coinAmount = coinAmountSO.Value;
    }
}
