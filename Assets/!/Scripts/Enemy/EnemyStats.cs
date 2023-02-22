using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class EnemyStats : MonoBehaviour
    {
        [Tooltip("This is the enemy stats scriptable object that this enemy will use. This is required.")]
        public SO.EnemyStatsSO enemyStats;
        [SerializeField]
        private float _health;
        [SerializeField]
        private float _armor;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _attackSpeed;
        [SerializeField]
        private int _attackRange;
        [SerializeField]
        private float _attackDamage;
        [HideInInspector]
        public UnityEvent<float> OnHealthChanged;
        [HideInInspector]
        public UnityEvent<float> OnArmorChanged;
        [HideInInspector]
        public UnityEvent<float> OnSpeedChanged;
        [HideInInspector]
        public UnityEvent<float> OnAttackSpeedChanged;
        [HideInInspector]
        public UnityEvent<int> OnAttackRangeChanged;
        [HideInInspector]
        public UnityEvent<float> OnAttackDamageChanged;

        private void Start()
        {
            _health = enemyStats.health;
            _armor = enemyStats.armor;
            _speed = enemyStats.speed;
            _attackSpeed = enemyStats.attackSpeed;
            _attackRange = enemyStats.attackRange;
            _attackDamage = enemyStats.attackDamage;
        }
        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                OnHealthChanged.Invoke(_health);
            }
        }
        public float Armor
        {
            get => _armor;
            set
            {
                _armor = value;
                OnArmorChanged.Invoke(_armor);
            }
        }
        public float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnSpeedChanged.Invoke(_speed);
            }
        }
        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                _attackSpeed = value;
                OnAttackSpeedChanged.Invoke(_attackSpeed);
            }
        }
        public int AttackRange
        {
            get => _attackRange;
            set
            {
                _attackRange = value;
                OnAttackRangeChanged.Invoke(_attackRange);
            }
        }
        public float AttackDamage
        {
            get => _attackDamage;
            set
            {
                _attackDamage = value;
                OnAttackDamageChanged.Invoke(_attackDamage);
            }
        }

    }

}