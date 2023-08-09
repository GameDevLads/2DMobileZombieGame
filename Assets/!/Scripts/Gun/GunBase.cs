using Assets.Scripts;
using Assets.Scripts.Gun.Data;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Gun
{
    public abstract class GunBase : MonoBehaviour, IPlayerWeapon
    {
        [Tooltip("The gun bullet trail.")]
        public TrailRenderer TrailRenderer;

        [Tooltip("The gun muzzle.")]
        public GameObject Muzzle;

        [Tooltip("The UI ammo text game object.")]
        public GameObject UIAmmoText;

        protected AudioClip _gunShotAudioClip;
        protected Sprite _muzzleFlash;
        protected float _bulletShootDistance = 10f;
        protected float _shootDelay = 0.1f;
        protected float _bulletSpreadAngle = 0.1f;
        protected int _numberOfBulletsToFire = 1;
        protected float _lastTimeGunFired = Mathf.NegativeInfinity;
        protected float _damagePerImpact;
        protected float _trailStartWidth;
        protected float _trailEndWidth;
        protected float _trailTime;
        protected Vector2 _bulletScale;
        protected bool _isParentFacingRight;
        protected int _ammoChamberCount;
        protected int _ammoChamberMax;
        protected int _totalAmmoCount;
        protected float _gunReloadStartTime = Mathf.NegativeInfinity;
        protected float _reloadDelay = 1.5f;
        protected bool _isReloading = false;

        protected virtual void StartBase()
        {
            InitData(null);
            UpdateAmmoText();
        }

        protected virtual void UpdateBase()
        {
            _isParentFacingRight = transform.parent.gameObject.transform.localScale.x > 0;
        }

        void Start() => StartBase();
        void Update() => UpdateBase();

        private bool CanFire()
            => IsShootDelayFinished()
            && ChamberHasAmmo();

        private bool IsShootDelayFinished() => _lastTimeGunFired + _shootDelay < Time.time;

        private bool ChamberHasAmmo() => _ammoChamberCount > 0;

        private bool IsReloading()
        {
            if (_ammoChamberCount <= 0 && _totalAmmoCount > 0)
            {
                if (!_isReloading)
                {
                    _isReloading = true;
                    _gunReloadStartTime = Time.time;
                    UpdateAmmoText(" - Reloading");
                }

                if (_gunReloadStartTime + _reloadDelay > Time.time)
                    return true; // Do not reload until we finish delay
                else
                {
                    // Reloading is finished
                    _isReloading = false;
                    var ammoToAddToChamber = _ammoChamberMax > _totalAmmoCount
                        ? _totalAmmoCount
                        : _ammoChamberMax;

                    _totalAmmoCount -= ammoToAddToChamber;
                    _ammoChamberCount = ammoToAddToChamber;
                    UpdateAmmoText();
                }

                return true;
            }

            return false;
        }

        private void UpdateAmmoText(string suffix = "")
        {
            if (UIAmmoText == null)
                return;

            var textMesh = UIAmmoText.GetComponent<TextMesh>();

            if (textMesh == null)
                return;

            var textBuilder = new StringBuilder();
            textBuilder.Append(_ammoChamberCount)
                .Append("/")
                .Append(_totalAmmoCount);

            if (!string.IsNullOrEmpty(suffix))
                textBuilder.Append(suffix);

            textMesh.text = textBuilder.ToString();
        }

        protected virtual void InitData(GunData gunData)
        {
            _shootDelay = gunData.ShootDelay;
            _bulletSpreadAngle = gunData.BulletSpreadAngle;
            _numberOfBulletsToFire = gunData.NumberOfBulletsToFire;
            _bulletShootDistance = gunData.BulletShootDistance;
            _muzzleFlash = gunData.MuzzleFlash;
            _damagePerImpact = gunData.DamagePerImpact;
            _bulletScale = gunData.BulletScale;
            _trailStartWidth = gunData.TrailStartWidth;
            _trailEndWidth = gunData.TrailEndWidth;
            _trailTime = gunData.TrailTime;
            _gunShotAudioClip = gunData.GunShotAudioClip;
            _ammoChamberCount = gunData.AmmoChamberCount;
            _totalAmmoCount = gunData.TotalAmmoCount;
            _ammoChamberMax = gunData.AmmoChamberCount;
            TrailRenderer.colorGradient = gunData.BulletTrailGradient;
        }

        public virtual void FireGun()
        {
            if (IsReloading())
                return; // We need to reload even when there is no input from the user

            if (!CanFire())
                return; // We can't shoot if we don't meet all conditions

            var numberOfBulletsToFire = _numberOfBulletsToFire > _ammoChamberCount
                ? _ammoChamberCount : _numberOfBulletsToFire;

            _lastTimeGunFired = Time.time;

            if (_gunShotAudioClip != null)
                AudioSource.PlayClipAtPoint(_gunShotAudioClip, Muzzle.transform.position);

            for (int i = 0; i < numberOfBulletsToFire; i++)
            {
                _ammoChamberCount--;
                UpdateAmmoText();

                var muzzlePosition = Muzzle.transform.position;
                StartCoroutine(SpawnMuzzleFlash());

                var direction = GetShootDirection(Muzzle.transform.right);

                // We need to ignore the player colliders 
                var raycastHit = Physics2D.RaycastAll(muzzlePosition, direction, _bulletShootDistance)
                    .FirstOrDefault(x => !string.Equals(x.collider.tag, "AutoAim") && !string.Equals(x.collider.tag, "Player"));

                if (raycastHit.collider != null)
                { // if we have hit a surface then spawn trail
                    var currentRotation = GetCurrentRotation(raycastHit.point);
                    var trail = InitTrail(TrailRenderer, Muzzle.transform.position, currentRotation);

                    StartCoroutine(SpawnTrail(trail, raycastHit.point, () => ApplyDamage(raycastHit)));
                }
                else
                {
                    var forwardDir = Muzzle.transform.position + direction * _bulletShootDistance;
                    var currentRotation = GetCurrentRotation(forwardDir);
                    var trail = InitTrail(TrailRenderer, Muzzle.transform.position, currentRotation);

                    StartCoroutine(SpawnTrail(trail, forwardDir, () => ApplyDamage(raycastHit)));
                }
            }
        }

        protected virtual void ApplyDamage(RaycastHit2D raycastHit)
        {
            if (raycastHit.collider != null && string.Equals(raycastHit.collider.tag, "Enemy"))
            {
                var enemyComponent = raycastHit.collider.GetComponent<Enemy>();
                if (enemyComponent != null)
                    enemyComponent.ApplyDamage(_damagePerImpact);
            }
        }

        private Quaternion GetCurrentRotation(Vector3 forwardDir)
        {
            var dir = forwardDir - Muzzle.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var angleAxis = Quaternion.AngleAxis(angle, Muzzle.transform.forward);
            var currentRotation = Quaternion.Slerp(Muzzle.transform.rotation, angleAxis, 1f);
            return currentRotation;
        }

        private Vector3 GetShootDirection(Vector3 muzzle)
        {
            var direction = new Vector3(muzzle.x, muzzle.y, muzzle.z);

            if (!_isParentFacingRight)
            {
                direction.x *= -1;
                direction.y *= -1;
            }

            if (_bulletSpreadAngle > 0)
            {
                var angleAxis = Quaternion.AngleAxis(Random.Range(-_bulletSpreadAngle, _bulletSpreadAngle), Muzzle.transform.forward);
                var currentRotation = Quaternion.Slerp(Muzzle.transform.rotation, angleAxis, 1f);

                return currentRotation * direction;
            }

            return direction;
        }

        private TrailRenderer InitTrail(TrailRenderer trailRenderer, Vector3 position, Quaternion rotation)
        {
            var trail = Instantiate(trailRenderer, position, rotation);
            trail.transform.localScale = _bulletScale;
            trail.GetComponent<TrailRenderer>().startWidth = _trailStartWidth;
            trail.GetComponent<TrailRenderer>().endWidth = _trailEndWidth;
            trail.GetComponent<TrailRenderer>().time = _trailTime;
            return trail;
        }

        private IEnumerator SpawnMuzzleFlash()
        {
            var spriteRenderer = Muzzle.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = _muzzleFlash;
            }
            else
            {
                spriteRenderer = Muzzle.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = _muzzleFlash;
            }

            var framesFlashed = 0;

            while (framesFlashed <= 60)
            {
                framesFlashed++;
                yield return null;
            }

            if (spriteRenderer != null)
                spriteRenderer.sprite = null;
        }

        private IEnumerator SpawnTrail(TrailRenderer trailRenderer, Vector2 point, System.Action onCollide = null)
        {
            float time = 0;
            Vector3 startPosition = trailRenderer.transform.position;

            while (time < 1)
            {
                trailRenderer.transform.position = Vector3.Lerp(startPosition, point, time);
                time += Time.deltaTime / trailRenderer.time;

                yield return null;
            }
            
            if (trailRenderer != null)
            {
                trailRenderer.transform.position = point;
                Destroy(trailRenderer.gameObject);
            }

            if (onCollide != null)
                onCollide();

            // TODO: We have collided with our point so play an animation
        }

        public void UseWeapon(Vector2 direction)
        {
            FireGun();
        }
    }
}