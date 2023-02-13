using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gun.Data
{
    [CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunDataScriptableObject", order = 1)]
    public class GunData : ScriptableObject
    {
        [Tooltip("The delay between shots.")]
        public float ShootDelay = 0.1f;

        [Tooltip("The bullet spread.")]
        public float BulletSpreadAngle = 0.1f;

        [Tooltip("Number of bullets to fire per update.")]
        public int NumberOfBulletsToFire = 1;

        [Tooltip("The bullet shoot distance.")]
        public float BulletShootDistance = 10f;

        [Tooltip("The amount of damage that is dealt to the enemy per impact.")]
        public float DamagePerImpact = 10f;

        [Tooltip("The bullet trail gradient to use.")]
        public Gradient BulletTrailGradient;

        [Tooltip("The bullet trail start width.")]
        public float TrailStartWidth = 0.2f;

        [Tooltip("The bullet trail end width.")]
        public float TrailEndWidth = 0f;

        [Tooltip("The bullet trail time.")]
        public float TrailTime = 0.1f;

        [Tooltip("The amout of ammo a single chamber can hold before reloading.")]
        public int AmmoChamberCount = 30;

        [Tooltip("The total amount of ammo that the gun has.")]
        public int TotalAmmoCount = 60;

        [Tooltip("The size of the bullet.")]
        public Vector2 BulletScale = new Vector2(0.2f, 0.2f);

        [Tooltip("The gun muzzle flash sprite.")]
        public Sprite MuzzleFlash;

        [Tooltip("The gun shot audio clip.")]
        public AudioClip GunShotAudioClip;

        [Tooltip("The gun has infinite ammo.")]
        public bool HasInfiniteAmmo = false;
    }
}