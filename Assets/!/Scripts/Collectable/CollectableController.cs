using Assets.__.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectableController : MonoBehaviour, ICollectable
{
    public FloatVariableSO coinAmountSO;
    public FloatVariableSO collectableLifetimeSO;
    public FloatVariableSO blinkingDelayedStartSO;
    public FloatVariableSO blinkingIntervalSO;

    public float DropChancePercentage { get; set; } = 10;

    private void Start()
    {
        StartCoroutine(Blink());
        Destroy(gameObject, collectableLifetimeSO.Value);
    }
  
    private IEnumerator Blink()
    {
        yield return new WaitForSeconds(blinkingDelayedStartSO.Value);
        while (true)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
            yield return new WaitForSeconds(blinkingIntervalSO.Value);
            GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(blinkingIntervalSO.Value);
        }       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            coinAmountSO.Value++;
            Destroy(gameObject);
        }
    }
    
}
