using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "New Projectile Ability", menuName = "ScriptableObjects/Abilities/Projectile Ability")]
    public class ProjectileAbilitySO : Ability
    {
        public GameObject WeaponPrefab;
        private Scripts.Interfaces.IPlayerWeapon playerWeapon;

        public override void Init(GameObject obj)
        {
            var playerHands = obj.transform.GetChild(0);
            playerWeapon = Instantiate(WeaponPrefab, playerHands).GetComponent<Scripts.Interfaces.IPlayerWeapon>();
            CooldownManager.Instance.StartCooldown(this);
        }

        public override void Reset()
        {
            ResetLevel();
            if (playerWeapon != null)
            {
                CooldownManager.Instance.StopCooldown(this);
                Destroy((playerWeapon as MonoBehaviour).gameObject);
                Destroy(playerWeapon as MonoBehaviour);
            }
        }

        public override void TriggerAbility()
        {
            if(playerWeapon != null)
                playerWeapon.UseWeapon(AutoAim.Instance.GetAimDirection() * 10);
        }

    }
}