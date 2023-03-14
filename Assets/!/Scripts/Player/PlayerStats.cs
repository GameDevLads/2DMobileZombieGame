using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public FloatVariableSO coinAmountSO;
    
    [SerializeField]
    private float _coinAmount;
    private void Update()
    {
        _coinAmount = coinAmountSO.Value;
    }
}
