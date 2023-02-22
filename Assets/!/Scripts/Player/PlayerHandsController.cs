using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
    private bool _facingRight = true;
    private float angle;
    public void UpdatePositionMouse(Vector2 pointerPosition)
    {
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        pointerPosition.x = pointerPosition.x - objectPos.x;
        pointerPosition.y = pointerPosition.y - objectPos.y;

        transform.localScale = pointerPosition.x > 0 ? Vector3.one : new Vector3(1, -1, -1);

        angle = Mathf.Atan2(pointerPosition.y, pointerPosition.x) * Mathf.Rad2Deg;

        if (_facingRight)
            angle = Mathf.Atan2(pointerPosition.y, pointerPosition.x) * Mathf.Rad2Deg;
        else
            angle = Mathf.Atan2(pointerPosition.y * -1, pointerPosition.x * -1) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void UpdatePositionGamepad(Vector2 pointerPosition)
    {
        angle = Mathf.Atan2(pointerPosition.y, pointerPosition.x) * Mathf.Rad2Deg;
        var lookRotation = Quaternion.Euler(angle * Vector3.forward);
        transform.rotation = lookRotation;
        transform.localScale = pointerPosition.x > 0 ? Vector3.one : new Vector3(1, -1, -1);
    }
}
