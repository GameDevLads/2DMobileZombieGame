using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public FloatVariableSO coinAmountSO;
    public BoolVariableSO keyPressedESO;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(keyPressedESO.Value == true && coinAmountSO.Value >= 1)
        {
            Destroy(gameObject);
            coinAmountSO.Value--;
        }
        else if (keyPressedESO.Value == true && coinAmountSO.Value < 1)
        {
            Debug.Log("NOT ENOUGH COINS RETARD!");
        }
    }
}
