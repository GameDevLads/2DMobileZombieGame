using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableControllerV2 : MonoBehaviour
{
    [SerializeField] private CollectableV2 collectable;
    private void Start()
    {
        StartCoroutine(DespawnAnimation());
        GetComponent<SpriteRenderer>().sprite = collectable.defaultModel;
        Destroy(gameObject, collectable.collectableLifetime);
    }

    private IEnumerator DespawnAnimation()
    {
        yield return new WaitForSeconds(collectable.blinkingDelayedStart);
        GetComponent<SpriteRenderer>().sprite = collectable.despawnModel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collectable.IncrementCollectable();
            Destroy(gameObject);
        }
    }
}
