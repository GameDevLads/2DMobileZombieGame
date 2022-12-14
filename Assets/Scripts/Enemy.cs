using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [Tooltip("The maximum health the enemy has.")]
        public float Health = 100f;

        private GameObject _healthText;
        private float _totalHealth;
        private const string _healthTextFormat = "Health: {0}/{1}";

        private void Start()
        {
            var position = transform.position;
            _healthText = new GameObject();
            _healthText.transform.position = position;
            _healthText.transform.rotation = Quaternion.identity;
            _healthText.transform.SetParent(transform);
            _healthText.transform.localPosition = new Vector2(-2f, 1.5f);

            var healthText = _healthText.AddComponent<TextMesh>();
            healthText.text = string.Format(_healthTextFormat, Health, Health);
            healthText.fontSize = 55;
            healthText.color = Color.yellow;
            _healthText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            _totalHealth = Health;
        }

        private void SetHealthText()
        {
            var healthText = _healthText.GetComponent<TextMesh>();
            healthText.text = string.Format(_healthTextFormat, Health, _totalHealth);
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

            Health = Health - damage;
            SetHealthText();

            if (Health <= 0)
            {
                Destroy(_healthText);
                Destroy(gameObject);
            }

            Destroy(damageObj, 1f);
        }
    }
}
