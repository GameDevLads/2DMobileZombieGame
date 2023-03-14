using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "New Projectile Ability", menuName = "ScriptableObjects/Abilities/Projectile Ability")]
    public class ProjectileAbilitySO : Ability
    {
        public GameObject WeaponPrefab;
        private Interfaces.IPlayerWeapon playerWeapon;

        public override void Init(GameObject obj)
        {
            var playerHands = obj.transform.GetChild(0);
            playerWeapon = Instantiate(WeaponPrefab, playerHands).GetComponent<Interfaces.IPlayerWeapon>();
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
            playerWeapon?.UseWeapon(AutoAim.Instance.GetAimDirection() * 10);
        }

    }
}