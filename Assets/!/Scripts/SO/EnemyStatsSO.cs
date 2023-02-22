using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "New Enemy Stats", menuName = "ScriptableObjects/Enemy Stats", order = 1)]
    public class EnemyStatsSO : ScriptableObject
    {
        public string enemyName;
        public int health;
        public int armor;
        public float speed;
        public float attackSpeed;
        public int attackRange;
        public int attackDamage;

    }
}