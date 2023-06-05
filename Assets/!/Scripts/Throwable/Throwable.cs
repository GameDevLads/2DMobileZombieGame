using Assets.Scripts.Interfaces;
using Assets.Scripts.Throwable.Data;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Throwable
{
    public class Throwable : MonoBehaviour, IPlayerWeapon
    {
        [Tooltip("The area explosion game object.")]
        public GameObject AreaExplosion;

        public ThrowableData ThrowableData;
        private bool _isThrown = false;
        private float _damageDealtStartTime = Mathf.NegativeInfinity; 

        public void UseWeapon(Vector2 direction)
        {
            Throw(direction);
        }

        private void Update()
        {
            if (!_isThrown || ThrowableData.Explode) // Don't deal damage for explosive throwables as they deal damage on impact
                return;

            var damageArea = Physics2D.OverlapCircleAll(gameObject.transform.position, ThrowableData.DamageRadius);

            if (!damageArea.Any(x => string.Equals(x.tag, "Enemy")))
                return; // No enemies to deal damage to

            if (_damageDealtStartTime + ThrowableData.DamageTime > Time.time)
                return; // Start dealing damage to enemies and set damage dealt time

            _damageDealtStartTime = Time.time;

            foreach (var collider in damageArea)
            {
                if (string.Equals(collider.tag, "Enemy"))
                {
                    var enemyComponent = collider.GetComponent<Enemy>();
                    if (enemyComponent != null)
                        enemyComponent.ApplyDamage(ThrowableData.Damage);
                }
            }
        }

        /// <summary>
        /// Throw the weapon in the direction we are pointing.
        /// </summary>
        /// <param name="pointerPosition"></param>
        public void Throw(Vector2 pointerPosition)
        {
            if (_isThrown)
                return;

            _damageDealtStartTime = Mathf.NegativeInfinity;
            _isThrown = true;
            var mouseWorldDirection = (pointerPosition.ConvertTo<Vector3>() - Camera.main.transform.position).normalized;
            var mouseRay = new Ray(Camera.main.transform.position, mouseWorldDirection);
            
            var raycastHit = Physics2D.RaycastAll(gameObject.transform.position, mouseRay.direction, ThrowableData.ThrowDistance)
                .FirstOrDefault(x => !string.Equals(x.collider.tag, "Player"));

            Vector2 throwPosition;
            var parentObj = gameObject.transform.parent;
            var worldPos = gameObject.transform.position;

            if (raycastHit.collider != null)
                throwPosition = raycastHit.point;
            else // we haven't hit anything so throw the weapon where the player points.
                throwPosition = pointerPosition;

            gameObject.transform.SetParent(null, false); // Detach throwable from player.

            AddTrailRenderer();
            StartCoroutine(ThrowStart(worldPos, throwPosition, () =>
            {
                if (ThrowableData.ShouldReturn)
                    ThrowableReturn(parentObj);
                else if (ThrowableData.Explode)
                {
                    StartCoroutine(StartExplosion(ThrowableData.ExplosionCountDown));
                }
                else
                    Destroy(gameObject, 3f);
            }));
        }

        /// <summary>
        /// Method that is called when the throwable comes back to the player.
        /// </summary>
        /// <param name="parentObj"></param>
        private void ThrowableReturn(Transform parentObj)
        {
            StartCoroutine(ThrowEnd(gameObject.transform.position, parentObj, () => AttachThrowableToPlayer(parentObj)));
        }

        /// <summary>
        /// Attaches the throwable object to the player once it comes back.
        /// </summary>
        /// <param name="parentObj"></param>
        private void AttachThrowableToPlayer(Transform parentObj)
        {
            gameObject.transform.SetParent(parentObj, false);
            var localThrowablePosition = transform.InverseTransformPoint(gameObject.transform.position);
            StartCoroutine(ThrowableEndTransition(localThrowablePosition, new Vector2(1.5f, 0f)));
            RemoveTrailRenderer();
        }

        /// <summary>
        /// Throwable is thrown from it's starting point to the end point.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="onFinish"></param>
        /// <returns></returns>
        private IEnumerator ThrowStart(Vector2 startPoint, Vector2 endPoint, System.Action onFinish = null)
        {
            float time = 0;

            while (time < 1)
            {
                transform.position = Vector3.Lerp(startPoint, endPoint, time);
                transform.Rotate(new Vector3(0f, 0f, -1f), Time.deltaTime * ThrowableData.RotateSpeed);

                Debug.DrawLine(startPoint, endPoint, UnityEngine.Color.magenta);

                time += Time.deltaTime / ThrowableData.DistanceTravelSpeed;

                yield return null;
            }
            transform.position = endPoint;

            if (onFinish != null)
                onFinish();
        }

        /// <summary>
        /// Throwable is coming back to the player. 
        /// We use the parent object here instead of an end point vector as the player is moving so the transition is smoother.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="parentObj"></param>
        /// <param name="onFinish"></param>
        /// <returns></returns>
        private IEnumerator ThrowEnd(Vector2 startPoint, Transform parentObj, System.Action onFinish = null)
        {
            float time = 0;
            while (time < 1)
            {
                transform.position = Vector3.Lerp(startPoint, parentObj.transform.position, time);
                transform.Rotate(new Vector3(0f, 0f, -1f), Time.deltaTime * ThrowableData.RotateSpeed);
                time += Time.deltaTime / ThrowableData.DistanceTravelSpeed;

                yield return null;
            }
            transform.position = parentObj.gameObject.transform.position;

            if (onFinish != null)
                onFinish();
        }

        /// <summary>
        /// Add smooth transition when the throwable comes back to the user.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private IEnumerator ThrowableEndTransition(Vector2 startPoint, Vector2 endPoint)
        {
            float time = 0;
            while (time < 1)
            {
                gameObject.transform.localPosition = Vector3.Lerp(startPoint, endPoint, time);
                time += Time.deltaTime / ThrowableData.ThrowablePlayerTransitionSpeed;

                yield return null;
            }
            transform.localPosition = endPoint;
            _isThrown = false;
        }

        /// <summary>
        /// Starts the explosion if the object should explode and deal damage
        /// </summary>
        private IEnumerator StartExplosion(float time)
        {
            yield return new WaitForSeconds(time);

            var explosionPosition = gameObject.transform.position;
            var areaExplosion = Instantiate(AreaExplosion, explosionPosition, Quaternion.identity);
            var diameter = ThrowableData.ExplosionRadiusStart * 2;
            areaExplosion.transform.localScale = new Vector2(diameter, diameter);
            var animationLength = areaExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            gameObject.GetComponent<Renderer>().enabled = false;
            DealAreaDamage(explosionPosition, ThrowableData.ExplosionRadiusEnd);
            StartCoroutine(SpawnExplosion(animationLength, areaExplosion, () =>
            {
                Destroy(gameObject);
                Destroy(areaExplosion);
            }));
        }

        /// <summary>
        /// Spawns the explosion with the start radius and ends with end radius
        /// </summary>
        /// <param name="explosionTime"></param>
        /// <param name="explosionObject"></param>
        /// <param name="onCollide"></param>
        /// <returns></returns>
        private IEnumerator SpawnExplosion(float explosionTime, GameObject explosionObject, System.Action onCollide = null)
        {
            float time = 0;
            var startRadius = new Vector2(ThrowableData.ExplosionRadiusStart * 2, ThrowableData.ExplosionRadiusStart * 2);
            var endRadius = new Vector2(ThrowableData.ExplosionRadiusEnd * 2, ThrowableData.ExplosionRadiusEnd * 2);

            while (time < 1)
            {
                explosionObject.transform.localScale = Vector2.Lerp(startRadius, endRadius, time);
                time += Time.deltaTime / explosionTime;
                yield return null;
            }

            if (onCollide != null)
                onCollide();
        }

        /// <summary>
        /// Deals area damage to enemies for a configured explosion damage and end radius.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radius"></param>
        private void DealAreaDamage(Vector2 point, float radius)
        {
            var damageArea = Physics2D.OverlapCircleAll(point, radius);

            foreach (var collider in damageArea)
            {
                if (string.Equals(collider.tag, "Enemy"))
                {
                    var enemyComponent = collider.GetComponent<Enemy>();
                    if (enemyComponent != null)
                        enemyComponent.ApplyDamage(ThrowableData.ExplosionDamage);
                }
            }
        }

        private void AddTrailRenderer()
        {
            if (gameObject.GetComponent<TrailRenderer>() != null)
                return;

            var trailRenderer = gameObject.AddComponent<TrailRenderer>();

            trailRenderer.time = 1.0f;
            trailRenderer.startWidth = 0.1f;
            trailRenderer.endWidth = 0.01f;
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
            trailRenderer.colorGradient = new Gradient
            {
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1.0f, 0.0f),
                    new GradientAlphaKey(0.0f, 1.0f)
                },
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(Color.red, 0.0f),
                    new GradientColorKey(Color.yellow, 1.0f)
                }
            };
        }

        private void RemoveTrailRenderer()
        {
            var trailRenderer = gameObject.GetComponent<TrailRenderer>();

            if (trailRenderer == null)
                return;

            Destroy(trailRenderer, 0f);
        }
    }
}