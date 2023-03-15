using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class CooldownManager : MonoBehaviour
    {
        public static CooldownManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartCooldown(Ability ability)
        {
            StartCoroutine(Cooldown(ability));
        }

        public void StopCooldown(Ability ability)
        {
            StopCoroutine(Cooldown(ability));
        }

        private IEnumerator Cooldown(Ability ability)
        {
            WaitForSeconds wait = new(ability.BaseCooldown);
            while (true)
            {
                ability.TriggerAbility();
                yield return wait;
            }
        }
    }
}