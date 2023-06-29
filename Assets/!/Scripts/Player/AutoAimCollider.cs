using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimCollider : MonoBehaviour
{
    public Action<Collider2D> OnTriggerStay2D_Action;
    public CircleCollider2D cr;

    void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerStay2D_Action?.Invoke(collision);
    }
 
}
