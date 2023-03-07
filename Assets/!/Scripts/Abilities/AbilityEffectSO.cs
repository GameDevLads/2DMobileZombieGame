using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "New Ability Effect", menuName = "ScriptableObjects/Abilities/Ability Effect")]
    public class AbilityEffectSO : ScriptableObject
    {
        public Level Level1;
        public Level Level2;
        public Level Level3;
        public Level Level4;
        public Level Level5;
        [Tooltip("Prefab of the ability that will be spawned when the effect is applied")]
        public List<ActiveAbility> ActiveAbilities;

    }

    [System.Serializable]
    public class Level
    {
        public List<Modifier> Modifiers;
    }

    [System.Serializable]
    public class Modifier
    {
        public StatTypeSO StatType;
        public float Value;
        public Operation Operation;
        public Target Target;
    }

}