using UnityEngine;
using Assets.Scripts.Abilities;

namespace Assets.Scripts.Interfaces
{
    public interface IProjectileController
    {
        void UseWeapon(Vector2 direction);
        void ApplyModifier(ProjectileModifier modifier);
    }
}