
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{

    [CreateAssetMenu(fileName = "New Stats Object", menuName = "ScriptableObjects/Stats/Stats")]
    public class StatsSO : ScriptableObject
    {
        public Stats BaseStats;
        public Stats Stats;

        public void Reset()
        {
            Stats = BaseStats;
        }

        public void ApplyEffect(Abilities.AbilityEffectSO effect)
        {
            // switch (effect.Operation)
            // {
            //     case Operation.Add:
            //         Stats.SetValue(effect.StatType.Value, Stats.GetValue(effect.StatType.Value) + effect.Value);
            //         break;
            //     case Operation.Multiply:
            //         Stats.SetValue(effect.StatType.Value, Stats.GetValue(effect.StatType.Value) * effect.Value);
            //         break;
            //     case Operation.Divide:
            //         Stats.SetValue(effect.StatType.Value, Stats.GetValue(effect.StatType.Value) / effect.Value);
            //         break;
            //     case Operation.Subtract:
            //         Stats.SetValue(effect.StatType.Value, Stats.GetValue(effect.StatType.Value) - effect.Value);
            //         break;
            //     default:
            //         break;
            // }
        }

    }

    [System.Serializable]
    public class Stats
    {
        public float Health;
        public float Stamina;
        public float Strength;
        public float Armor;
        public float MagicResist;
        public float AttackSpeed;
        public float AttackRange;
        public float MovementSpeed;
        public float CriticalChance;
        public float CriticalDamage;
        public float LifeSteal;

        public void SetValue(string propertyName, float value)
        {
            switch (propertyName)
            {
                case "Health":
                    Health = value;
                    break;
                case "Stamina":
                    Stamina = value;
                    break;
                case "Strength":
                    Strength = value;
                    break;
                case "Armor":
                    Armor = value;
                    break;
                case "MagicResist":
                    MagicResist = value;
                    break;
                case "AttackSpeed":
                    AttackSpeed = value;
                    break;
                case "AttackRange":
                    AttackRange = value;
                    break;
                case "MovementSpeed":
                    MovementSpeed = value;
                    break;
                case "CriticalChance":
                    CriticalChance = value;
                    break;
                case "CriticalDamage":
                    CriticalDamage = value;
                    break;
                case "LifeSteal":
                    LifeSteal = value;
                    break;
                default:
                    throw new System.Exception($"Property {propertyName} does not exist");
            }
        }

        public float GetValue(string propertyName)
        {
            switch (propertyName)
            {
                case "Health":
                    return Health;
                case "Stamina":
                    return Stamina;
                case "Strength":
                    return Strength;
                case "Armor":
                    return Armor;
                case "MagicResist":
                    return MagicResist;
                case "AttackSpeed":
                    return AttackSpeed;
                case "AttackRange":
                    return AttackRange;
                case "MovementSpeed":
                    return MovementSpeed;
                case "CriticalChance":
                    return CriticalChance;
                case "CriticalDamage":
                    return CriticalDamage;
                case "LifeSteal":
                    return LifeSteal;
                default:
                    throw new System.Exception($"Property {propertyName} does not exist");
            }
        }
    }

}