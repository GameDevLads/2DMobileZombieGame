using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class CooldownManager : MonoBehaviour
    {
        public List<Ability> abilities = new List<Ability>();
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
            WaitForSeconds wait = new WaitForSeconds(ability.BaseCooldown);
            while (true)
            {
                ability.TriggerAbility();
                yield return wait;
            }
        }
    }
}