using UnityEngine;

namespace Assets.__.Scripts.Enemy.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 5)]
    public class EnemyData : ScriptableObject
    {
        [Tooltip("Enables and shows the damage text that is displayed above the enemy to show damage dealt.")]
        public bool EnableVisualDamageText = false;

        [Tooltip("Enables and shows the healthbar above enemy.")]
        public bool EnableHealthbar = true;

        [Tooltip("The collectable pool is used to drop collectables for an enemy.")]
        public GameObject CollectablePool;
    }
}