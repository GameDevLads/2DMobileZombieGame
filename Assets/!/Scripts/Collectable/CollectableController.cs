using Assets.__.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectableController : MonoBehaviour, ICollectable
{
    public FloatVariableSO CollectableAmountSO;
    public FloatVariableSO CollectableLifetimeSO;
    public FloatVariableSO BlinkingDelayedStartSO;
    public FloatVariableSO BlinkingIntervalSO;

    [field: SerializeField]
    public FloatVariableSO DropChancePercentage { get; set; }

    private void Start()
    {
        StartCoroutine(Blink());
        Destroy(gameObject, CollectableLifetimeSO.Value);
    }
  
    // TODO: Enable/Disable blinking based on config
    private IEnumerator Blink()
    {
        yield return new WaitForSeconds(BlinkingDelayedStartSO.Value);
        while (true)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
            yield return new WaitForSeconds(BlinkingIntervalSO.Value);
            GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(BlinkingIntervalSO.Value);
        }       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CollectableAmountSO.Value++;
            Destroy(gameObject);
        }
    }
    
}
