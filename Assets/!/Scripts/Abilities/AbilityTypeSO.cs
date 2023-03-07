using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    namespace Assets.Scripts.Abilities
    {
        
    [CreateAssetMenu(fileName = "New Ability Type", menuName = "ScriptableObjects/Abilities/Ability Type")]
    public class AbilityTypeSO : ScriptableObject
    {
        public string Value;
    }
    }