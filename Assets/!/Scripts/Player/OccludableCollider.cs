using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccludableCollider : MonoBehaviour
{
    public Action<Collider2D> OnTriggerEnter2D_Action;
    public Action<Collider2D> OnTriggerExit2D_Action;

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2D_Action?.Invoke(collision);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit2D_Action?.Invoke(collision);
    }
}
