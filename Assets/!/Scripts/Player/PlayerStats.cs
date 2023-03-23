using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public FloatVariableSO coinAmountSO;
    public FloatVariableSO xpAmountSO;

    public float coinAmount;
    [SerializeField] private float _xpAmmount;
    private void Start()
    {
        xpAmountSO.Value = 0;
    }
    private void Update()
    {
        coinAmount = coinAmountSO.Value;
        _xpAmmount = xpAmountSO.Value;
    }
}
