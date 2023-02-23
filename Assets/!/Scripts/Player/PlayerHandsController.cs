using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
    private float _angle;
    public void UpdatePositionMouse(Vector2 pointerPosition)
    {
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        pointerPosition.x = pointerPosition.x - objectPos.x;
        pointerPosition.y = pointerPosition.y - objectPos.y;

        transform.localScale = pointerPosition.x > 0 ? Vector3.one : new Vector3(1, -1, -1);
        _angle = Mathf.Atan2(pointerPosition.y, pointerPosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
    }

    public void UpdatePositionGamepad(Vector2 pointerPosition)
    {
        //this keeps the position of the weapon at the last point before rotation stopped instead of resetting to the default position
        if (pointerPosition == Vector2.zero) return;

        transform.localScale = pointerPosition.x > 0 ? Vector3.one : new Vector3(1, -1, -1);
        _angle = Mathf.Atan2(pointerPosition.y, pointerPosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(_angle * Vector3.forward); ;
    }
}
