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
            float cooldown = ability.GetCooldown();
            if (cooldown <= 0)
            {
                Debug.LogWarning($"Cooldown for {ability.name} is 0 or less.");
                yield break;
            }
            WaitForSeconds wait = new(cooldown);
            while (true)
            {
                ability.TriggerAbility();
                yield return wait;
            }
        }
    }
}