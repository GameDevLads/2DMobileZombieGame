using UnityEngine;
using Slider = UnityEngine.UI.Slider;
using TMPro;
using System.Collections;
using Assets.__.Scripts.Enemy.Data;
using Assets.__.Scripts.Interfaces;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public IntVariableSO enemiesKilledSO;
        public IntVariableSO enemiesOnScreenSO;

        public Stats.StatsSO StatsSO;
        
        [Tooltip("The maximum health the enemy has.")]
        public float Health;

        [Tooltip("The enemy healthbar.")]
        public GameObject HealthBar;

        [Tooltip("The enemy configuration data.")]
        public EnemyData EnemyData;

        private float _totalHealth;
        private const string _healthTextFormat = "Health: {0}/{1}";
        private Slider _healthSlider;
        private TextMeshProUGUI _healthTextPro;
        private ICollectablePool _collectablePool;

        private void Start()
        {
            Health = StatsSO.CurrentStats.Health;
            _totalHealth = Health;
            AddHealthBar();
            AddCollectablePool();
        }

        public void ApplyDamage(float damage)
        {
            Health = Health - damage;

            if (EnemyData.EnableVisualDamageText)
                DisplayDamageDealtText(damage);

            if (HealthBar != null && EnemyData.EnableHealthbar)
                StartCoroutine(InitHealthDepletion());

            if (Health <= 0)
            {
                Destroy(gameObject);
                DropCollectable();
                enemiesKilledSO.Value++;
                enemiesOnScreenSO.Value--;
            }
        }

        /// <summary>
        /// Drops collectable when enemy dies.
        /// </summary>
        private void DropCollectable()
        {
            if (_collectablePool == null)
                return;

            _collectablePool.DropCollectable();
        }

        /// <summary>
        /// Shows the damage text that has been dealt to the enemy.
        /// </summary>
        /// <param name="damage"></param>
        private void DisplayDamageDealtText(float damage)
        {
            var damageObj = new GameObject();
            damageObj.transform.position = transform.position;
            damageObj.transform.rotation = Quaternion.identity;
            damageObj.transform.SetParent(transform);

            var damageText = damageObj.AddComponent<TextMesh>();
            damageText.text = $"-{damage}";
            damageText.fontSize = 65;
            damageObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Destroy(damageObj, 1f);
        }

        /// <summary>
        /// Add collectable pool that's responsible for dropping collectables based on chance %.
        /// </summary>
        private void AddCollectablePool()
        {
            if (EnemyData.CollectablePool == null)
                return;

            var collectablePool = Instantiate(EnemyData.CollectablePool, this.gameObject.transform.position, this.gameObject.transform.rotation);
            collectablePool.transform.SetParent(this.gameObject.transform);
            _collectablePool = GetComponentInChildren<ICollectablePool>();
        }

        /// <summary>
        /// Add a healthbar above the enemy collider.
        /// </summary>
        private void AddHealthBar()
        {
            if (HealthBar == null || !EnemyData.EnableHealthbar)
                return;

            var healthObj = Instantiate(HealthBar, this.gameObject.transform.position, this.gameObject.transform.rotation);
            healthObj.transform.SetParent(this.gameObject.transform);
            var currentPosition = healthObj.transform.localPosition;
            var enemyCollider = this.GetComponent<BoxCollider2D>();

            if (enemyCollider != null)
            { // Calculate the position of the health bar based on the enemy collider if present.
                Vector2 localCenter = transform.InverseTransformPoint(enemyCollider.bounds.center);
                RectTransform rectTransform = healthObj.GetComponent<RectTransform>();
                float healthBarYPos = localCenter.y + (enemyCollider.size.y / 2) + rectTransform.rect.height;
                currentPosition = new Vector2(0f, healthBarYPos);
            }
            else
            {
                currentPosition.y = 1.5f;
            }

            healthObj.transform.localPosition = currentPosition;
            _healthSlider = healthObj.GetComponentInChildren<Slider>();
            _healthTextPro = healthObj.GetComponentInChildren<TextMeshProUGUI>();

            _healthSlider.maxValue = _totalHealth;
            _healthSlider.value = _totalHealth;
            _healthTextPro.SetText(string.Format(_healthTextFormat, Health, _totalHealth));
        }

        /// <summary>
        /// Initiate health depletion will animate the health bar going down when damage is taken.
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitHealthDepletion()
        {
            float time = 0;
            var startVal = _healthSlider.value;

            while (time < 1)
            {
                time += Time.deltaTime / 0.1f;
                var newHealth = Mathf.Lerp(startVal, Health, time);

                _healthSlider.value = newHealth;
                _healthTextPro.SetText(string.Format(_healthTextFormat, (int)newHealth, _totalHealth));

                yield return null;
            }
            _healthSlider.value = Health;
        }
    }
}