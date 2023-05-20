using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.Abilities
{
    public class CanvasAbility : MonoBehaviour
    {
        public GameObject Name;
        public GameObject Image;
        public GameObject Description;
        // public GameObject Level;
        public Button Button;

        public void SetAbility(Ability ability)
        {
            Name.GetComponent<TextMeshProUGUI>().text = ability.Name;
            Image.GetComponent<Image>().sprite = ability.Icon;
            Description.GetComponent<TextMeshProUGUI>().text = ability.GetDescription(ability.CurrentLevel + 1);
            // Level.GetComponent<TextMeshProUGUI>().text = ability.CurrentLevel.ToString();
            Button.onClick.AddListener(() => AbilityManager.Instance.OnButtonPressed(ability));
        }
        public void RemoveListeners()
        {
            Button.onClick.RemoveAllListeners();
        }
        

    }
}