using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ZombieClasses
{
    public class TankZombie : MonoBehaviour
    {
        private enum State { Normal, Enraged };
        private float _health;
        [SerializeField]
        private State _state;
        private EnemyStats _enemyStats;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _state = State.Normal;
            _enemyStats = GetComponent<EnemyStats>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            _enemyStats.OnHealthChanged.AddListener(UpdateHealth);
        }
        private void OnDisable()
        {
            _enemyStats.OnHealthChanged.RemoveListener(UpdateHealth);
        }
        private void UpdateHealth(float health)
        {
            _health = health;
            if (_health <= 50 && _state == State.Normal)
            {
                Rage();
            }
        }
        private void Rage()
        {
            _state = State.Enraged;
            _spriteRenderer.color = new Color(1, 0.5f, 0.5f);
            _enemyStats.Speed *= 2;
            _enemyStats.AttackSpeed *= 2;
            _enemyStats.AttackDamage *= 2;
        }

    }

}