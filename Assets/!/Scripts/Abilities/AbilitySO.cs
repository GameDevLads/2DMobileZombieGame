using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "ScriptableObjects/Abilities/Ability")]
    public class AbilitySO : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public AbilityEffectSO Effects;
        public bool IsUnlocked;
        public List<AbilityEffectSO> GetAbilityEffects(int level)

        {
            // switch (level)
            // {
            //     case 1:
            //         return Effects.Level1;
            //     case 2:
            //         return Effects.Level2;
            //     case 3:
            //         return Effects.Level3;
            //     case 4:
            //         return Effects.Level4;
            //     case 5:
            //         return Effects.Level5;
            //     default:
            //         return null;
            // }
            return null;
        }

    }

}