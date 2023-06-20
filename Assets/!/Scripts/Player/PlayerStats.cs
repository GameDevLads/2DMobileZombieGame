using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public FloatVariableSO coinAmountSO;
    public FloatVariableSO xpAmountSO;
    public FloatVariableSO BrowniePointsSO;

    public float coinAmount;

    [SerializeField] 
    private float _xpAmmount;

    private void Start()
    {
        xpAmountSO.Value = 0;
        CreatePlayerStatsText();
    }

    private void Update()
    {
        coinAmount = coinAmountSO.Value;
        _xpAmmount = xpAmountSO.Value;

        _coinAmountText.text = $"Coins: {coinAmountSO.Value}";
        _xpAmountText.text = $"XP: {xpAmountSO.Value}";
        _browniePointsText.text = $"Brownie Points: {BrowniePointsSO.Value}";
    }

    private TextMesh _coinAmountText;
    private TextMesh _xpAmountText;
    private TextMesh _browniePointsText;

    private void CreatePlayerStatsText()
    {
        _coinAmountText = AddTextGameObject($"Coins: {coinAmountSO.Value}", 2.0f);
        _xpAmountText = AddTextGameObject($"XP: {xpAmountSO.Value}", 3.0f);
        _browniePointsText = AddTextGameObject($"Brownie Points: {BrowniePointsSO.Value}", 4.0f);

        TextMesh AddTextGameObject(string text, float yPos)
        {
            var playerStatsObject = new GameObject();
            playerStatsObject.transform.position = transform.position;
            playerStatsObject.transform.rotation = Quaternion.identity;
            playerStatsObject.transform.SetParent(transform);

            var textMesh = playerStatsObject.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.fontSize = 65;
            playerStatsObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            playerStatsObject.transform.localPosition = new Vector3(0.1f, yPos, 0.1f);
            return textMesh;
        }
    }
}
