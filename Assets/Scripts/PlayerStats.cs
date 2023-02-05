using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public FloatVariableSO coinAmountSO;

    public float coinAmount;
    private void Update()
    {
        coinAmount = coinAmountSO.Value;
    }
}
