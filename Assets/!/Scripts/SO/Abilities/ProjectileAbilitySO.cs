using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "New Projectile Ability", menuName = "ScriptableObjects/Abilities/Projectile Ability")]
    public class ProjectileAbilitySO : Ability
    {
        [Tooltip("The prefab that holds the projectile controller script; it will be instantiated on the player's hands")]
        public GameObject ProjectileControllerPrefab;
        
        [Tooltip("Levels describe the buffs/nerfs that the ability will have at each level; each level is applied in addition to the previous level")]
        public ProjectileAbilityLevel Level1;
        public ProjectileAbilityLevel Level2;
        public ProjectileAbilityLevel Level3;
        public ProjectileAbilityLevel Level4;
        public ProjectileAbilityLevel Level5;
        private Interfaces.IProjectileController _projectileController;

        public override string GetDescription(int level)
        {
            return level switch
            {
                1 => Level1.Description,
                2 => Level2.Description,
                3 => Level3.Description,
                4 => Level4.Description,
                5 => Level5.Description,
                _ => null,
            };
        }
        public override void Init(GameObject obj)
        {
            var playerHands = obj.transform.GetChild(0);
            _projectileController = Instantiate(ProjectileControllerPrefab, playerHands).GetComponent<Interfaces.IProjectileController>();
            CooldownManager.Instance.StartCooldown(this);
        }

        public override void Upgrade()
        {
            IncreaseLevel();
            ApplyModifiers();
        }

        private void ApplyModifiers()
        {
            switch (CurrentLevel)
            {
                case 1:
                    Level1.ApplyModifiers(_projectileController);
                    break;
                case 2:
                    Level2.ApplyModifiers(_projectileController);
                    break;
                case 3:
                    Level3.ApplyModifiers(_projectileController);
                    break;
                case 4:
                    Level4.ApplyModifiers(_projectileController);
                    break;
                case 5:
                    Level5.ApplyModifiers(_projectileController);
                    break;
            }
        }

        public override float GetCooldown()
        {
            return CurrentLevel switch
            {
                1 => Level1.CooldownSeconds,
                2 => Level2.CooldownSeconds,
                3 => Level3.CooldownSeconds,
                4 => Level4.CooldownSeconds,
                5 => Level5.CooldownSeconds,
                _ => 0,
            };
        }

        public override void Reset()
        {
            ResetLevel();
            if (_projectileController != null)
            {
                CooldownManager.Instance.StopCooldown(this);
                Destroy((_projectileController as MonoBehaviour).gameObject);
                Destroy(_projectileController as MonoBehaviour);
            }
        }

        public override void TriggerAbility()
        {
        }


    }

    [System.Serializable]
    public class ProjectileModifier 
    {
        
        public Operation Operation;
        [Tooltip(@"Stat Type is a ScriptableObject that holds the name of the stat that should be modified;
if you want to add a new stat, you need to create a new ScriptableObject for it, as well as add a `case` to each operation in the relevant Projectile Controller script 
(see /Assets/!/Scripts/Abilities/Controllers/RotatingWeapons.cs)")]
        public Stats.StatTypeSO StatType;
        public float Value;

    }

    [System.Serializable]
    public class ProjectileAbilityLevel // Helper class to make it easier to access the modifiers and description for each level
    {
        [Tooltip("Description of the buff/nerf that this level provides; Level 1 should describe what the ability does")]
        public string Description;
        [Tooltip("Leave this at 0 for passives; you can set different values for each level if you want")]
        public float CooldownSeconds;
        [Tooltip("A modifier describes how a stat should be modified (e.g. add 10% to damage)")]
        public List<ProjectileModifier> Modifiers;

        /// <summary>
        ///Sends the relevant modifiers to the given projectile controller which will apply them to the projectile;
        /// </summary>  
        public void ApplyModifiers(Interfaces.IProjectileController _projectileController)
        {
            Modifiers.ForEach(modifier =>
            {
                _projectileController.ApplyModifier(modifier);
            });
        }
    }
}