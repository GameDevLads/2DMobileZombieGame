using System.Collections;
using System.Collections.Generic;
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
            switch (level)
            {
                case 1:
                    return Level1.Modifiers;
                case 2:
                    return Level2.Modifiers;
                case 3:
                    return Level3.Modifiers;
                case 4:
                    return Level4.Modifiers;
                case 5:
                    return Level5.Modifiers;
                default:
                    return null;
            }
        }
        public override void Init(GameObject gameObject = null)
        {
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