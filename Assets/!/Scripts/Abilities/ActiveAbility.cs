using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public abstract class ActiveAbility
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public float Cooldown;
        public float CurrentCooldown;
        public bool IsOnCooldown;
        public bool IsReady;
        public bool IsCasting;

        public abstract void Cast();
        public abstract void Update();

    }
}