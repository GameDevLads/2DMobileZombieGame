using System.Collections.Generic;
using Assets.Scripts.Stats;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "New Passive Ability", menuName = "ScriptableObjects/Abilities/Passive Ability")]
    public class PassiveAbilitySO : Ability
    {
        public Level Level1;
        public Level Level2;
        public Level Level3;
        public Level Level4;
        public Level Level5;

        private List<ModifierSO> GetModifiers(int level)
        {
            return level switch
            {
                1 => Level1.Modifiers,
                2 => Level2.Modifiers,
                3 => Level3.Modifiers,
                4 => Level4.Modifiers,
                5 => Level5.Modifiers,
                _ => null,
            };
        }
        public override void Init(GameObject gameObject = null)
        {
        }
        public override void Upgrade()
        {
            IncreaseLevel();
            ApplyPassiveEffects();
        }
        public override void Reset()
        {
            ResetLevel();
        }
        public override void TriggerAbility()
        {
            // Do nothing
        }
        private List<StatsSO> MatchingTargets(Abilities.ModifierSO modifier)
        {
            var targets = new List<StatsSO>();
            AbilityManager.Instance.EnemyStats.ForEach(enemy =>
            {
                if (modifier.MatchesCriteria(enemy.Tags))
                {
                    targets.Add(enemy);
                }
            });
            if (modifier.MatchesCriteria(AbilityManager.Instance.PlayerStats.Tags))
            {
                targets.Add(AbilityManager.Instance.PlayerStats);
            }
            return targets;
        }

        private void ApplyPassiveEffects()
        {
            GetModifiers(CurrentLevel).ForEach(modifier =>
            {
                MatchingTargets(modifier).ForEach(target => target.ApplyModifier(modifier));
            });
        }

    }

    [System.Serializable]
    public class Level
    {
        public List<ModifierSO> Modifiers;
    }
}