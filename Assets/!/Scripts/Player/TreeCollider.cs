using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollider : MonoBehaviour
{
    public Action<Collision2D> OnCollisionEnter2D_Action;
    public Action<Collision2D> OnCollisionExit2D_Action;

    void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2D_Action?.Invoke(collision);
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionExit2D_Action?.Invoke(collision);
    }

}
