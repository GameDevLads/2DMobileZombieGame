using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager instance;

        public StatsSO _playerStats;
        public StatsSO _enemyStats;

        [Tooltip("List of all abilities in the game")]
        public List<Ability> Abilities = new List<Ability>();
        private List<Abilities.AbilityTypeSO> _activeAbilities = new List<Abilities.AbilityTypeSO>();
        public List<Abilities.AbilityTypeSO> ActiveAbilities => _activeAbilities;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
        }


        public void AddOrLevelUpAbility(Abilities.AbilityTypeSO type)
        {
            var ability = Abilities.Find(x => x.Type.Value == type.Value);
            if (ability == null)
            {
                _activeAbilities.Add(ability.Type);
            }
            ability.Level++;
        }

        public List<Ability> GetRandomAbilities(int count, bool excludeMaxLevel = false)
        {
            var abilities = new List<Ability>();
            for (int i = 0; i < count; i++)
            {
                var ability = Abilities[Random.Range(0, Abilities.Count)];
                if (excludeMaxLevel)
                {
                    if (ability.Level < 5)
                    {
                        abilities.Add(ability);
                    }
                }
                else
                {
                    abilities.Add(ability);
                }
            }
            return abilities;
        }

        public void ApplyEffects(List<Abilities.AbilityEffectSO> effects)
        {
            // foreach (var effect in effects)
            // {
            //     switch (effect.Target)
            //     {
            //         case Target.Player:
            //             _playerStats.ApplyEffect(effect);
            //             break;
            //         case Target.Enemy:
            //             _enemyStats.ApplyEffect(effect);
            //             break;
            //         default:
            //             break;
            //     }
            // }
        }


    //     public void RemoveEffect(Effect effect)
    //     {
    //         _activeEffects.Remove(effect);
    //         // _playerStats.RemoveEffect(effect);
    //     }

        public int GetAbilityLevel(Abilities.AbilitySO ability)
        {
            int level = 0;
    //         foreach (var item in _activeAbilities)
    //         {
    //             if (item == ability)
    //             {
    //                 level++;
    //             }
    //         }
            return level;
        }

    }

    [System.Serializable]
    public class Ability
    {
        public Abilities.AbilityTypeSO Type;
        public int Level;
        public Abilities.AbilitySO AbilitySO;
        public List<Abilities.AbilityEffectSO> Effects {
            get
            {
                return AbilitySO.GetAbilityEffects(Level);
            }
        }

    }


}