using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Abilities
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Modifier", menuName = "ScriptableObjects/Abilities/Modifier")]
    public class ModifierSO : ScriptableObject
    {
        [Tooltip("The stat to modify. This should not be empty.")]
        public Stats.StatTypeSO StatType;
        public Operation Operation;
        public float Value;
        [Tooltip(@"Enter a set of tags and their required values as criteria for selecting the target of this ability. 
Only entities with matching tags will be affected. For example, entering 'Enemy: true, Ranged: true' will only target ranged enemies.")]
        public TagDictionary Target;
        /// <summary>
        /// Returns true if the given list of tags matches the criteria for this modifier.
        /// </summary>
        public bool MatchesCriteria(List<string> tags)
        {
            foreach (KeyValuePair<string, bool> pair in Target)
            {
                if (pair.Value && !tags.Contains(pair.Key))
                    return false;
                else if (!pair.Value && tags.Contains(pair.Key))
                    return false;
            }
            return true;
        }
    }
}