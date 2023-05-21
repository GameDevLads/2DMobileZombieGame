using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Stats;
using Assets.Scripts.Collectable;

namespace Assets.Scripts.Abilities
{

    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager Instance;

        [Tooltip("The player's stats ScriptableObject")]
        public StatsSO PlayerStats;
        [Tooltip("List of all enemy stats ScriptableObjects")]
        public List<StatsSO> EnemyStats = new();

        [Tooltip("List of all abilities in the game")]
        public List<Ability> Abilities;
        [SerializeField]
        private readonly List<Ability> _activeAbilities = new();
        public List<Ability> ActiveAbilities => _activeAbilities;
        [Tooltip("The GameObject that holds the ability picker UI")]
        public GameObject AbilityPicker;
        [Tooltip("The child of the ability picker")]
        public GameObject UIAbilities;
        private readonly Queue<int> levelsToProcess = new();

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


        private void OnEnable()
        {
            XPManager.LevelUp += OnLevelUp;
        }

        private void OnDisable()
        {
            XPManager.LevelUp -= OnLevelUp;
        }

        private void OnLevelUp()
        {
            levelsToProcess.Enqueue(1); // Enqueue one level to process

            // If we're not currently processing a level up, start processing
            if (levelsToProcess.Count == 1)
            {
                PresentAbilities();
            }
        }

        private void PresentAbilities()
        {
            PauseManager.Instance.TogglePause();
            var abilities = GetRandomAbilities(3, true);
            AbilityPicker.SetActive(true);
            CanvasAbility[] canvasAbilities = UIAbilities.GetComponentsInChildren<CanvasAbility>();
            for (int i = 0; i < abilities.Count; i++)
            {
                canvasAbilities[i].SetAbility(abilities[i]);
            }
        }

        public void OnButtonPressed(Ability ability)
        {
            GameObject player = GameObject.FindWithTag("Player");
            AddOrLevelUpAbility(ability, player);
            AbilityPicker.SetActive(false);
            RemoveButtonListeners();
            PauseManager.Instance.TogglePause();

            // Remove the level we just processed
            levelsToProcess.Dequeue();

            // If there are more levels to process, present the next set of abilities
            if (levelsToProcess.Count > 0)
            {
                PresentAbilities();
            }
        }



        private void Init()
        {
            // reset all abilities and stats
            AbilityPicker.SetActive(false);
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
                ability.IncreaseLevel();
                ability.Init(gameObject);
                return;
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

        private void RemoveButtonListeners()
        {
            CanvasAbility[] canvasAbilities = UIAbilities.GetComponentsInChildren<CanvasAbility>();
            foreach (var canvasAbility in canvasAbilities)
            {
                canvasAbility.RemoveListeners();
            }
        }

    }

}