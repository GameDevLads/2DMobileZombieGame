using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    [SerializeField]
    private Vector2 dir;
    public float magnitude;
    private Rigidbody2D rb;
    private Animator animator;
    private float timeToLive = 3f;
    [SerializeField]
    private float timeAlive = 0f;
    private float damage = 1f;
    private float animationDuration = 1f;
    private bool isExploding = false;
    public GameObject Puddle;
    public Transform target;
    private Vector3 initialScale;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player").transform;
        dir = target.position - transform.position;
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (isExploding)
            return;
        // Modify the velocity of the projectile over time
        dir.y = Mathf.Lerp(dir.y, -1, Time.deltaTime);
        rb.velocity = ( dir * magnitude ) * 0.3f;
        timeAlive += Time.deltaTime;
        if (timeAlive > timeToLive)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            Explode();
            // instantiate puddle prefab
            GameObject puddle = (GameObject)Instantiate(Puddle, transform.position, Quaternion.Euler(0,0,30 * Random.Range(0, 12)));
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        if (other.gameObject.name == "Player")
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            DamagePlayer();
        }
    }
    void Explode()
    {
        isExploding = true;
        animator.SetTrigger("Explode");
        StartCoroutine(DestroyAfterTime(animationDuration));
        
    }
    void DamagePlayer()
    {
        Explode();
        //TODO: Damage player
    }
    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(animationDuration);
        StartCoroutine(DestroyAfterTime(timeToLive));
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * 0.5f;
            spriteRenderer.color = color;
            yield return null;
        }
    }

}