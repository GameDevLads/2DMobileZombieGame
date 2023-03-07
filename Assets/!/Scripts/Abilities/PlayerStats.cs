using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public enum StatType
    {
        Health,
        Stamina,
        Strength,
        Armor,
        MagicResist,
        AttackSpeed,
        AttackRange,
        MovementSpeed,
        CriticalChance,
        CriticalDamage,
        LifeSteal
    }
    public class PlayerStats: MonoBehaviour
    {
        public StatsSO StatsSO;


        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gameObject.name == "Zombie3")
                {
                    Debug.Log("Zombie3");
                    // AbilityManager.instance.AddEffect(effect);
                }
            }
        }


    }
}