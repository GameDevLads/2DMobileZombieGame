using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager Instance;

        public Stats.StatsSO PlayerStats;
        public List<Stats.StatsSO> EnemyStats = new();

        [Tooltip("List of all abilities in the game")]
        public List<Ability> Abilities;
        [SerializeField]
        private readonly List<Ability> _activeAbilities = new();
        public List<Ability> ActiveAbilities => _activeAbilities;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            Init();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Add Ability"))
            {
                var ability = GetRandomAbilities(1)[0];
                var player = GameObject.Find("Player");
                AddOrLevelUpAbility(ability, player);
            }
            if (GUI.Button(new Rect(10, 50, 100, 30), "Reset"))
            {
                Init();
            }
        }

        private void Init()
        {
            Abilities.ForEach(ability => ability.Reset());
            _activeAbilities.Clear();
            EnemyStats.ForEach(enemy => enemy.Reset());
            PlayerStats.Reset();
        }


        /// <summary>
        /// Adds an ability to the list of active abilities or levels up an existing ability
        /// </summary>
        public void AddOrLevelUpAbility(Ability ability, GameObject gameObject = null)
        {
            if (ability.CurrentLevel == 0)
            {
                _activeAbilities.Add(ability);
                ability.Init(gameObject);
            }
            ability.Upgrade();
        }

        /// <summary>
        /// Returns a list of random abilities from the list of all abilities
        /// </summary>
        public List<Ability> GetRandomAbilities(int count, bool excludeMaxLevel = false)
        {
            var abilities = new List<Ability>();
            for (int i = 0; i < count; i++)
            {
                var ability = Abilities[Random.Range(0, Abilities.Count)];
                if (excludeMaxLevel)
                {
                    if (ability.CurrentLevel < 5)
                        abilities.Add(ability);
                }
                else
                    abilities.Add(ability);
            }
            return abilities;
        }


    }

}