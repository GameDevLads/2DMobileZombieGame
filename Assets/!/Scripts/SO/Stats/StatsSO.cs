
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Stats
{

    [CreateAssetMenu(fileName = "New Stats Object", menuName = "ScriptableObjects/Stats/Stats")]
    public class StatsSO : ScriptableObject
    {
        [Tooltip("Tags must be TitleCase")]
        public List<string> Tags;
        public Stats CurrentStats = new();
        public Stats BaseStats = new();
        public int Level = 1;

        public void Reset()
        {
            CurrentStats = new Stats
            {
                Health = BaseStats.Health,
                Strength = BaseStats.Strength,
                Armor = BaseStats.Armor,
                MagicResist = BaseStats.MagicResist,
                AttackSpeed = BaseStats.AttackSpeed,
                MovementSpeed = BaseStats.MovementSpeed,
                CriticalChance = BaseStats.CriticalChance,
                CriticalDamage = BaseStats.CriticalDamage,
                LifeSteal = BaseStats.LifeSteal,
                PickupRange = BaseStats.PickupRange,
                CooldownReduction = BaseStats.CooldownReduction,
                HealthRegeneration = BaseStats.HealthRegeneration,
                AttackRangeMultiplier = BaseStats.AttackRangeMultiplier,
                XPMultiplier = BaseStats.XPMultiplier
            };
            Level = 1;
        }
        public void ApplyModifier(Abilities.ModifierSO modifier)
        {
            switch (modifier.Operation)
            {
                case Operation.Add:
                    CurrentStats.SetValue(modifier.StatType.Value, CurrentStats.GetValue(modifier.StatType.Value) + modifier.Value);
                    break;
                case Operation.Subtract:
                    CurrentStats.SetValue(modifier.StatType.Value, CurrentStats.GetValue(modifier.StatType.Value) - modifier.Value);
                    break;
                case Operation.Multiply:
                    CurrentStats.SetValue(modifier.StatType.Value, CurrentStats.GetValue(modifier.StatType.Value) * modifier.Value);
                    break;
                case Operation.Divide:
                    CurrentStats.SetValue(modifier.StatType.Value, CurrentStats.GetValue(modifier.StatType.Value) / modifier.Value);
                    break;
                default:
                    break;
            }
        }
        public void LevelUp()
        {
            Level++;
        }
    }

    [System.Serializable]
    public class Stats
    {
        public float Health;
        public float Strength;
        public float Armor;
        public float MagicResist;
        public float AttackSpeed;
        public float MovementSpeed;
        public float CriticalChance;
        public float CriticalDamage;
        public float LifeSteal;
        public float PickupRange;
        public float CooldownReduction;
        public float HealthRegeneration;
        public float AttackRangeMultiplier;
        public float XPMultiplier;

        public void SetValue(string propertyName, float value)
        {
            switch (propertyName)
            {
                case "Health":
                    Health = value;
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
                case "PickupRange":
                    PickupRange = value;
                    break;
                case "CooldownReduction":
                    CooldownReduction = value;
                    break;
                case "HealthRegeneration":
                    HealthRegeneration = value;
                    break;
                case "AttackRangeMultiplier":
                    AttackRangeMultiplier = value;
                    break;
                case "XPMultiplier":
                    XPMultiplier = value;
                    break;
                default:
                    throw new System.Exception($"Property {propertyName} does not exist");
            }
        }

        public float GetValue(string propertyName)
        {
            return propertyName switch
            {
                "Health" => Health,
                "Strength" => Strength,
                "Armor" => Armor,
                "MagicResist" => MagicResist,
                "AttackSpeed" => AttackSpeed,
                "MovementSpeed" => MovementSpeed,
                "CriticalChance" => CriticalChance,
                "CriticalDamage" => CriticalDamage,
                "LifeSteal" => LifeSteal,
                "PickupRange" => PickupRange,
                "CooldownReduction" => CooldownReduction,
                "HealthRegeneration" => HealthRegeneration,
                "AttackRangeMultiplier" => AttackRangeMultiplier,
                "XPMultiplier" => XPMultiplier,
                _ => throw new System.Exception($"Property {propertyName} does not exist"),
            };
        }
    }

}