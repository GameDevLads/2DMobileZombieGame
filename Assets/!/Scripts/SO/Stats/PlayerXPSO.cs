using UnityEngine;

namespace Assets.Scripts.Collectable
{
    [CreateAssetMenu(fileName = "Player XP", menuName = "ScriptableObjects/PlayerXP")]
    public class PlayerXPSO : ScriptableObject
    {
        public XPData CurrentXP = new XPData();
        public XPData BaseXP = new XPData();
        public void AddXP(float amount)
        {
            CurrentXP.Amount += amount;
        }

        public void LevelUp()
        {
            CurrentXP.Level++;
            CurrentXP.Amount -= CurrentXP.LevelUpAmount;
            CurrentXP.LevelUpAmount *= CurrentXP.LevelUpMultiplier;
        }

        public void Reset()
        {
            CurrentXP.Amount = BaseXP.Amount;
            CurrentXP.Level = BaseXP.Level;
            CurrentXP.LevelUpAmount = BaseXP.LevelUpAmount;
            CurrentXP.LevelUpMultiplier = BaseXP.LevelUpMultiplier;
        }


    }

    [System.Serializable]
    public class XPData
    {
        public float Amount;
        public float Level;
        public float LevelUpAmount;
        public float LevelUpMultiplier;
    }
}