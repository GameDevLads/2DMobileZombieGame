using UnityEngine;
namespace Assets.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Stat Type", menuName = "ScriptableObjects/Stats/Stat Type")]
    public class StatTypeSO : ScriptableObject
    {
        [Tooltip("The name of the stat in TitleCase")]
        public string Value;
    }
}