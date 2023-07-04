using TMPro;
using UnityEngine;

namespace Assets.__.Scripts.Events.HUD
{
    public class HUDWaveChanged : MonoBehaviour
    {
        public IntVariableSO CurrentWaveSO;

        public void WaveChanged()
        {
            var currentText = gameObject.GetComponent<TextMeshProUGUI>();
            currentText.text = $"Wave: {CurrentWaveSO.Value}";
        }
    }
}