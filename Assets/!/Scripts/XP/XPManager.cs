using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Collectable
{
    public class XPManager : MonoBehaviour
    {
        public Image XPBarFill;
        public TextMeshProUGUI LevelText;
        public PlayerXPSO PlayerXP;
        public delegate void OnLevelUp();
        public static event OnLevelUp LevelUp;
        public delegate void OnXPChange(float amount);
        public static event OnXPChange XPChange;

        public static void AddXP(float amount)
        {
            XPChange?.Invoke(amount);
        }

        public static void LevelUpEvent()
        {
            LevelUp?.Invoke();
        }

        private void OnEnable()
        {
            XPChange += UpdateXP;
            LevelUp += PlayerXP.LevelUp;
        }

        private void OnDisable()
        {
            XPChange -= UpdateXP;
            LevelUp -= PlayerXP.LevelUp;
        }

        private void UpdateXP(float amount)
        {
            PlayerXP.AddXP(amount);
            if (PlayerXP.CurrentXP.Amount >= PlayerXP.CurrentXP.LevelUpAmount)
            {
                LevelUp?.Invoke();
            }

            UpdateXPBar();
        }

        private void Start()
        {
            PlayerXP.Reset();
            UpdateXPBar();
        }


        private void UpdateXPBar()
        {
            float xpPercentage = PlayerXP.CurrentXP.Amount / PlayerXP.CurrentXP.LevelUpAmount;
            XPBarFill.fillAmount = xpPercentage;
            LevelText.text = PlayerXP.CurrentXP.Level.ToString();
        }
    }
}