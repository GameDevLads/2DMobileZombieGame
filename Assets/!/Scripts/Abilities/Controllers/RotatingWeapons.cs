using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Abilities;

public class RotatingWeapons : MonoBehaviour, IProjectileController
{
    /*
    * Each Projectile Ability should implement IProjectileController
    *
    */
    public RotatingWeapon WeaponPrefab;
    public int WeaponCount = 3;
    public float Radius = 2f;
    public float RotationSpeed = 200f;
    public float Damage = 1f;
    public float Knockback = 5f;
    private RotatingWeapon[] _weapons;

    private void Start()
    {
        _weapons = null;
        _weapons = new RotatingWeapon[WeaponCount];
        for (int i = 0; i < WeaponCount; i++)
        {
            _weapons[i] = Instantiate(WeaponPrefab);
            _weapons[i].transform.SetParent(transform);
        }
    }

    private void Update()
    {
        for (int i = 0; i < WeaponCount; i++)
        {
            var distance = 2 * Mathf.PI / WeaponCount;
            var angle = distance * i + Time.time * RotationSpeed;
            var x = Mathf.Cos(angle) * Radius;
            var y = Mathf.Sin(angle) * Radius;
            _weapons[i].transform.localPosition = new Vector3(x, y, 0);
            _weapons[i].Damage = Damage;
            _weapons[i].KnockbackForce = Knockback;
        }
    }
    private void AddWeapon(int count)
    {
        var newWeapons = new RotatingWeapon[WeaponCount];
        for (int i = 0; i < WeaponCount; i++)
        {
            if (i < _weapons.Length)
            {
                newWeapons[i] = _weapons[i];
            }
            else
            {
                newWeapons[i] = Instantiate(WeaponPrefab);
                newWeapons[i].transform.SetParent(transform);
            }
        }
        _weapons = newWeapons;
    }

    public void UseWeapon(Vector2 direction)
    {
    }

    public void AddModifier(ProjectileModifier modifier)
    {
        switch (modifier.StatType.Value)
        {
            case "Damage":
                Damage += modifier.Value;
                break;
            case "Speed":
                RotationSpeed += modifier.Value;
                break;
            case "Range":
                Radius += modifier.Value;
                break;
            case "WeaponCount":
                WeaponCount += (int)modifier.Value;
                AddWeapon((int)modifier.Value);
                break;
            case "Knockback":
                Knockback += modifier.Value;
                break;
            default:
                break;
        }
    }

    public void MultiplyModifier(ProjectileModifier modifier)
    {
        switch (modifier.StatType.Value)
        {
            case "Damage":
                Damage *= modifier.Value;
                break;
            case "Speed":
                RotationSpeed *= modifier.Value;
                break;
            case "Range":
                Radius *= modifier.Value;
                break;
            case "WeaponCount":
                WeaponCount *= (int)modifier.Value;
                break;
            case "Knockback":
                Knockback *= modifier.Value;
                break;
            default:
                break;
        }
    }

    public void SubtractModifier(ProjectileModifier modifier)
    {
        switch (modifier.StatType.Value)
        {
            case "Damage":
                Damage -= modifier.Value;
                break;
            case "Speed":
                RotationSpeed -= modifier.Value;
                break;
            case "Range":
                Radius -= modifier.Value;
                break;
            case "WeaponCount":
                WeaponCount -= (int)modifier.Value;
                break;
            case "Knockback":
                Knockback -= modifier.Value;
                break;
            default:
                break;
        }
    }

    public void DivideModifier(ProjectileModifier modifier)
    {
        switch (modifier.StatType.Value)
        {
            case "Damage":
                Damage /= modifier.Value;
                break;
            case "Speed":
                RotationSpeed /= modifier.Value;
                break;
            case "Range":
                Radius /= modifier.Value;
                break;
            case "WeaponCount":
                WeaponCount /= (int)modifier.Value;
                Start();
                break;
            case "Knockback":
                Knockback /= modifier.Value;
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        Damage = 1f;
        RotationSpeed = 200f;
        Radius = 2f;
        WeaponCount = 3;
        Knockback = 5f;
    }



}