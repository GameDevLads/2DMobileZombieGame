
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

        public void Reset()
        {
            CurrentStats = new Stats
            {
                Health = BaseStats.Health,
                Stamina = BaseStats.Stamina,
                Strength = BaseStats.Strength,
                Armor = BaseStats.Armor,
                MagicResist = BaseStats.MagicResist,
                AttackSpeed = BaseStats.AttackSpeed,
                AttackRange = BaseStats.AttackRange,
                MovementSpeed = BaseStats.MovementSpeed,
                CriticalChance = BaseStats.CriticalChance,
                CriticalDamage = BaseStats.CriticalDamage,
                LifeSteal = BaseStats.LifeSteal
            };
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
            return propertyName switch
            {
                "Health" => Health,
                "Stamina" => Stamina,
                "Strength" => Strength,
                "Armor" => Armor,
                "MagicResist" => MagicResist,
                "AttackSpeed" => AttackSpeed,
                "AttackRange" => AttackRange,
                "MovementSpeed" => MovementSpeed,
                "CriticalChance" => CriticalChance,
                "CriticalDamage" => CriticalDamage,
                "LifeSteal" => LifeSteal,
                _ => throw new System.Exception($"Property {propertyName} does not exist"),
            };
        }
    }

}