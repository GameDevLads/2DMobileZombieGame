using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public DoorMenu doorMenu;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        doorMenu.Pause();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        doorMenu.Resume();
    }
}
