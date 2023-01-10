using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerStatValues playerStatValues;

    public int coinAmount;
    private void Update()
    {
        coinAmount = playerStatValues.coinAmount;
    }
}
