
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class EnemyStats : MonoBehaviour
    {
        public StatsSO StatsSO;
        private float _health;
        private float _attackSpeed;
        private float _attackRange;
        private float _movementSpeed;

        private void Start()
        {
            StatsSO.Reset();
            _health = StatsSO.CurrentStats.Health;
            _attackSpeed = StatsSO.CurrentStats.AttackSpeed;
            _attackRange = StatsSO.CurrentStats.AttackRange;
            _movementSpeed = StatsSO.CurrentStats.MovementSpeed;

        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public float AttackSpeed
        {
            get { return _attackSpeed; }
            set { _attackSpeed = value; }
        }
        public float AttackRange
        {
            get { return _attackRange; }
            set { _attackRange = value; }
        }
        public float MovementSpeed
        {
            get { return _movementSpeed; }
            set { _movementSpeed = value; }
        }

    }
}