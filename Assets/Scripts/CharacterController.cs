using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    private bool _facingRight = true;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        if (mousePos.x < 0 && transform.localScale.x > 0)
        {
            _facingRight = false;
            // flip if we are facing right but mouse is on the left
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else if (mousePos.x > 0 && transform.localScale.x < 0)
        {
            _facingRight = true;
            // flip if mouse is on the right and we are facing left
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        if (_facingRight)
            angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        else
            angle = Mathf.Atan2(mousePos.y * -1, mousePos.x * -1) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
