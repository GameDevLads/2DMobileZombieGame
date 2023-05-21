using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Abilities;
using System.Collections.Generic;
using System;

public class RotatingWeapons : MonoBehaviour, IProjectileController
{
    public RotatingWeapon WeaponPrefab;
    public int WeaponCount = 3;
    public float Radius = 2f;
    public float RotationSpeed = 200f;
    public float Damage = 1f;
    public float Knockback = 5f;
    private RotatingWeapon[] _weapons;
    private Dictionary<Operation, Func<float, float, float>> operations;

    private void Start()
    {
        _weapons = null;
        _weapons = new RotatingWeapon[WeaponCount];
        for (int i = 0; i < WeaponCount; i++)
        {
            _weapons[i] = Instantiate(WeaponPrefab);
            _weapons[i].transform.SetParent(transform);
        }
        operations = new Dictionary<Operation, Func<float, float, float>>
        {
            {Operation.Add, (a, b) => a + b},
            {Operation.Multiply, (a, b) => a * b},
            {Operation.Subtract, (a, b) => a - b},
            {Operation.Divide, (a, b) => a / b},
        };
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
    private float ModifyStat(Operation operation, float stat, float modifierValue)
    {
        return operations[operation](stat, modifierValue);
    }

    /// <summary>
    /// Applies the relevant modifier stat to the weapon.
    /// </summary>
    public void ApplyModifier(ProjectileModifier modifier)
    {
        var operation = modifier.Operation;
        switch (modifier.StatType.Value)
        {
            case "Damage":
                Damage = ModifyStat(operation, Damage, modifier.Value);
                break;
            case "Speed":
                RotationSpeed = ModifyStat(operation, RotationSpeed, modifier.Value);
                break;
            case "Range":
                Radius = ModifyStat(operation, Radius, modifier.Value);
                break;
            case "WeaponCount":
                WeaponCount = (int)ModifyStat(operation, WeaponCount, (int)modifier.Value);
                if (operation == Operation.Add) AddWeapon((int)modifier.Value);
                break;
            case "Knockback":
                Knockback = ModifyStat(operation, Knockback, modifier.Value);
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