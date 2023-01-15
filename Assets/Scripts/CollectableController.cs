using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectableController : MonoBehaviour
{
    public Collectable collectable;
    //public PlayerStatValues playerStatValues;
    public IntVariableSO intVariableSO;

    private void Start()
    {
        StartCoroutine(Blink());
        Destroy(gameObject, collectable.collectableLifetime);
    }
  
    private IEnumerator Blink()
    {
        yield return new WaitForSeconds(collectable.blinkingDelayedStart);
        while (true)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
            yield return new WaitForSeconds(collectable.blinkingInterval);
            GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(collectable.blinkingInterval);
        }       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            intVariableSO.Value++;
            Destroy(gameObject);
        }
    }
    
}
