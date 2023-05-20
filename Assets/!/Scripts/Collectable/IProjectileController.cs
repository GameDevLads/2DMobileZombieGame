using UnityEngine;
using Assets.Scripts.Abilities;

namespace Assets.Scripts.Interfaces
{
    public interface IProjectileController
    {
        void UseWeapon(Vector2 direction);
        void AddModifier(ProjectileModifier modifier);
        void MultiplyModifier(ProjectileModifier modifier);
        void SubtractModifier(ProjectileModifier modifier);
        void DivideModifier(ProjectileModifier modifier);
    }
}