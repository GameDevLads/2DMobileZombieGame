using UnityEngine;
namespace Assets.Scripts.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public AudioClip Sound;
        [SerializeField]
        private int _currentLevel = 0;
        public bool IsUnlocked;
        public float BaseCooldown = 1f;

        public int CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
        }

        public void IncreaseLevel()
        {
            if (_currentLevel < 5)
            {
                _currentLevel++;
            }
        }
        public void ResetLevel()
        {
            _currentLevel = 0;
        }

        public void Unlock()
        {
            IsUnlocked = true;
        }

        public abstract void Upgrade();
        public abstract void Reset();
        public abstract void Init(GameObject gameObject = null);
        public abstract void TriggerAbility();

    }

}
