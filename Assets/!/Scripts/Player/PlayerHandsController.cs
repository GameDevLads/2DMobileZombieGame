using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
    private bool _facingRight = true;

    public void UpdatePosition(Vector2 pointerPosition)
    {
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        pointerPosition.x = pointerPosition.x - objectPos.x;
        pointerPosition.y = pointerPosition.y - objectPos.y;

        if (pointerPosition.x < 0 && transform.localScale.x > 0)
        {
            _facingRight = false;
            // flip if we are facing right but mouse is on the left
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else if (pointerPosition.x > 0 && transform.localScale.x < 0)
        {
            _facingRight = true;
            // flip if mouse is on the right and we are facing left
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        float angle = Mathf.Atan2(pointerPosition.y, pointerPosition.x) * Mathf.Rad2Deg;

        if (_facingRight)
            angle = Mathf.Atan2(pointerPosition.y, pointerPosition.x) * Mathf.Rad2Deg;
        else
            angle = Mathf.Atan2(pointerPosition.y * -1, pointerPosition.x * -1) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
