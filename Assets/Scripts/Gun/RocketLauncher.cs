using Assets.Scripts.Gun.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gun
{
    public class RocketLauncher : GunBase
    {
        [Tooltip("The area explosion game object.")]
        public GameObject AreaExplosion;

        [Tooltip("The explosion sprite.")]
        public GameObject ExplosionSprite;

        private bool _enableAreaDamage = false;
        private float _areaDamageRadiusStart = 0;
        private float _areaDamageRadiusEnd = 0;
        private float _areaDamage = 50f;
        protected AudioClip _explosionImpactAudioClip;

        public RocketLauncherData RocketLauncherData;

        protected override void InitData(GunData gunData)
        {
            _enableAreaDamage = RocketLauncherData.EnableAreaDamage;
            _areaDamageRadiusStart = RocketLauncherData.AreaDamageRadiusStart;
            _areaDamageRadiusEnd = RocketLauncherData.AreaDamageRadiusEnd;
            _areaDamage = RocketLauncherData.AreaDamage;
            _explosionImpactAudioClip = RocketLauncherData.ExplosionImpactAudioClip;
            base.InitData(RocketLauncherData);
        }

        protected override void ApplyDamage(RaycastHit2D raycastHit)
        {
            base.ApplyDamage(raycastHit); // Apply damage to enemy that was hit

            ApplyAreaDamageIfEnabled(raycastHit);
        }

        private void ApplyAreaDamageIfEnabled(RaycastHit2D raycastHit)
        {
            if (!_enableAreaDamage)
                return;

            if (raycastHit.collider != null)
            {
                var diameter = _areaDamageRadiusStart * 2;
                var areaExplosion = Instantiate(AreaExplosion, raycastHit.point, Quaternion.identity);
                areaExplosion.transform.localScale = new Vector2(diameter, diameter);

                #region TestExplosion
                //var areaExplosionSprite = Instantiate(ExplosionSprite, raycastHit.point, Quaternion.identity);
                //areaExplosionSprite.transform.localScale = new Vector2(_areaDamageRadius * 2, _areaDamageRadius * 2);
                //Destroy(areaExplosionSprite, 2f);
                #endregion

                var animationLength = areaExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

                if (_explosionImpactAudioClip != null)
                    AudioSource.PlayClipAtPoint(_explosionImpactAudioClip, raycastHit.point);

                StartCoroutine(SpawnExplosion(animationLength, areaExplosion, () =>
                {
                    Destroy(areaExplosion);
                }));

                var damageArea = Physics2D.OverlapCircleAll(raycastHit.point, _areaDamageRadiusEnd);

                foreach (var collider in damageArea)
                {
                    if (string.Equals(collider.tag, "Enemy"))
                    {
                        var enemyComponent = collider.GetComponent<Enemy>();
                        if (enemyComponent != null)
                            enemyComponent.ApplyDamage(_areaDamage);
                    }
                }
            }
        }

        private IEnumerator SpawnExplosion(float explosionTime, GameObject explosionObject, System.Action onCollide = null)
        {
            float time = 0;
            var startRadius = new Vector2(_areaDamageRadiusStart * 2, _areaDamageRadiusStart * 2);
            var endRadius = new Vector2(_areaDamageRadiusEnd * 2, _areaDamageRadiusEnd * 2);

            while (time < 1)
            {
                explosionObject.transform.localScale = Vector2.Lerp(startRadius, endRadius, time);
                time += Time.deltaTime / explosionTime;
                yield return null;
            }

            if (onCollide != null)
                onCollide();
        }
    }
}
