using UnityEngine;
using Assets.Scripts.Stats;

public class RotatingWeapon : MonoBehaviour
{
    public float Damage = 1f;
    public float KnockbackForce = 1f;
    private void Start()
    {
        AddTrailRenderer();
    }
    private void AddTrailRenderer()
        {
            if (gameObject.GetComponent<TrailRenderer>() != null)
                return;

            var trailRenderer = gameObject.AddComponent<TrailRenderer>();

            trailRenderer.time = 0.5f;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Assets.Scripts.Enemy>().ApplyDamage(Damage);
                Vector2 direction = other.transform.position - transform.position;
                other.gameObject.GetComponent<Assets.Scripts.EnemyMovement>().Knockback(direction.normalized, KnockbackForce);

            }
        }
}