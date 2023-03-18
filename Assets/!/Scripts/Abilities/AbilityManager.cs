using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Stats;

namespace Assets.Scripts.Abilities
{

    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager Instance;

        public StatsSO PlayerStats;
        public List<StatsSO> EnemyStats = new();

        [Tooltip("List of all abilities in the game")]
        public List<Ability> Abilities;
        [SerializeField]
        private readonly List<Ability> _activeAbilities = new();
        public List<Ability> ActiveAbilities => _activeAbilities;
        // the UI canvas that holds the ability buttons
        public GameObject AbilityPicker;
        public GameObject UIAbilities;
        public FloatVariableSO PlayerXPAmountSO;
        private float _nextLevelUpXP = 100;

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

        // private void OnGUI()
        // {
        //     if (GUI.Button(new Rect(10, 10, 100, 30), "Add Ability"))
        //     {
        //         var ability = GetRandomAbilities(1)[0];
        //         var player = GameObject.Find("Player");
        //         AddOrLevelUpAbility(ability, player);
        //     }
        //     if (GUI.Button(new Rect(10, 50, 100, 30), "Reset"))
        //     {
        //         Init();
        //     }
        // }
        private void Update()
        {
            if (PlayerXPAmountSO.Value >= _nextLevelUpXP)
            {
                _nextLevelUpXP *= 3;
                PlayerStats.LevelUp();
                PresentAbilities();
            }
        }

        private void Init()
        {
            AbilityPicker.SetActive(false);
            Abilities.ForEach(ability => ability.Reset());
            _activeAbilities.Clear();
            EnemyStats.ForEach(enemy => enemy.Reset());
            PlayerStats.Reset();
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


        /// <summary>
        /// Adds an ability to the list of active abilities or levels up an existing ability
        /// </summary>
        public void AddOrLevelUpAbility(Ability ability, GameObject gameObject = null)
        {
            if (ability.CurrentLevel == 0)
            {
                _activeAbilities.Add(ability);
                ability.Upgrade();
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

        public void OnButtonPressed(Ability ability)
        {
            var player = GameObject.Find("Player");
            AddOrLevelUpAbility(ability, player);
            AbilityPicker.SetActive(false);
            RemoveButtonListeners();
            PauseManager.Instance.TogglePause();
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