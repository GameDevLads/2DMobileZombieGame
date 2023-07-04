using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public FloatVariableSO coinAmountSO;
    public FloatVariableSO xpAmountSO;
    public FloatVariableSO BrowniePointsSO;

    private void Start()
    {
        xpAmountSO.Value = 0;
    }
}