using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]private Transform player;
    [SerializeField]private Vector3 offset;

    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
        }    
    }
    // allows to set target for the camera component
    public void setTarget(Transform target)
    {
        player = target;
    }
}
