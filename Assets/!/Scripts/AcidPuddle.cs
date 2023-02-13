using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPuddle : MonoBehaviour
{
    private float timeToLive = 5f;
    private float timeAlive = 0f;
    private float damage = 1f;
    [SerializeField]
    private bool isColiding = false;
    private float animationDuration = 4f;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(DamagePlayer());
        StartCoroutine(FadeOut());
    }
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > timeToLive)
            Destroy(gameObject);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
            isColiding = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
            isColiding = false;
    }
    IEnumerator DamagePlayer()
    {
        while (true)
        {
            if (isColiding)
            {
                // PlayerHealth playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
                // playerHealth.TakeDamage(damage);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(animationDuration);
        Color color = spriteRenderer.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * 0.5f;
            spriteRenderer.color = color;
            yield return null;
        }
    }

}
