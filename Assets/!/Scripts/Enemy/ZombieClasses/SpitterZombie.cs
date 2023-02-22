using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ZombieClasses
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyStats))]
    public class SpitterZombie : MonoBehaviour
    {
        public GameObject Acid;
        private EnemyMovement _enemyMovement;
        private EnemyStats _enemyStats;
        private float _attackSpeed;
        private Coroutine _spitCoroutine;

        private void Awake()
        {
            _enemyStats = GetComponent<EnemyStats>();
        }
        private void Start()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _attackSpeed = _enemyStats.AttackSpeed;
            _spitCoroutine = StartCoroutine(Spit(_attackSpeed));
        }

        IEnumerator Spit(float attackSpeed)
        {
            while (true)
            {
                var waitTime = 1 / attackSpeed;
                yield return new WaitForSeconds(waitTime);
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
                if (_enemyMovement.TargetInRange && !_enemyMovement.IsMoving)
                    Instantiate(Acid, pos, Quaternion.identity, transform);
            }
        }
    }
}