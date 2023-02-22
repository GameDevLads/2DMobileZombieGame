using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyStats))]
    public class Enemy : MonoBehaviour
    {
        private EnemyStats _enemyStats;
        private float _health;
        private GameObject _healthText;
        private float _totalHealth;
        private const string _healthTextFormat = "Health: {0}/{1}";

        private void Start()
        {
            _enemyStats = GetComponent<EnemyStats>();
            _health = _enemyStats.Health;
            var position = transform.position;
            _healthText = new GameObject();
            _healthText.transform.position = position;
            _healthText.transform.rotation = Quaternion.identity;
            _healthText.transform.SetParent(transform);
            _healthText.transform.localPosition = new Vector2(-2f, 1.5f);

            var healthText = _healthText.AddComponent<TextMesh>();
            healthText.text = string.Format(_healthTextFormat, _health, _health);
            healthText.fontSize = 55;
            healthText.color = Color.yellow;
            _healthText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            _totalHealth = _health;
        }

        private void SetHealthText()
        {
            var healthText = _healthText.GetComponent<TextMesh>();
            healthText.text = string.Format(_healthTextFormat, _health, _totalHealth);
        }

        public void ApplyDamage(float damage)
        {
            var damageObj = new GameObject();
            damageObj.transform.position = transform.position;
            damageObj.transform.rotation = Quaternion.identity;
            damageObj.transform.SetParent(transform);

            var damageText = damageObj.AddComponent<TextMesh>();
            damageText.text = $"-{damage}";
            damageText.fontSize = 65;
            damageObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            _health -= damage;
            _enemyStats.Health = _health;
            SetHealthText();

            if (_health <= 0)
            {
                Destroy(_healthText);
                Destroy(gameObject);
            }

            Destroy(damageObj, 1f);
        }
    }
}
