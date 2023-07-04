using UnityEngine;

namespace Assets.__.Scripts.SO.Player
{
    [CreateAssetMenu(fileName = "Player Weapon SO", menuName = "ScriptableObjects/Player/PlayerWeapon")]
    public class PlayerWeaponSO : ScriptableObject
    {
        [Tooltip("The player weapon.")]
        public GameObject Weapon;
    }
}