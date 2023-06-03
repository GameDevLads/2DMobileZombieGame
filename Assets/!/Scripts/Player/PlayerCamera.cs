using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]private Transform player;
    [SerializeField]private Vector3 offset;
    [SerializeField]private BoxCollider2D mapBounds;

    private float xMin, xMax, yMin, yMax; 
    private float camX, camY;
    private float camOrthographicSize;
    private float cameraRatio;
    private Camera mainCam;

    private void Start()
    {
        xMin = mapBounds.bounds.min.x; //get map bounds so camera doesnt follow outside those showing the blue background
        yMin = mapBounds.bounds.min.y;
        xMax = mapBounds.bounds.max.x;
        yMax = mapBounds.bounds.max.y;
        mainCam = GetComponent<Camera>();
        camOrthographicSize = mainCam.orthographicSize ; //vertical size of camera FOV

        //calculate the horizontal size based off the half size of the camera ortho and add that to the maximum size on the x of our bounds and divide that by 2.
        cameraRatio = ((xMax + camOrthographicSize) / 2.0f) - 2;//horizontal size of camera FOV, the -2 at the end increases the camera FOV enough to cover the entire map
    }
    private void FixedUpdate() 
    {
        camY = Mathf.Clamp(player.position.y, yMin + camOrthographicSize, yMax - camOrthographicSize);
        camX = Mathf.Clamp(player.position.x, xMin + cameraRatio, xMax - cameraRatio);
        if(player != null)
        {
            this.transform.position = new Vector3(camX + offset.x, camY + offset.y, this.transform.position.z);
        }
        
    }
    void Update()
    {
        if (player != null)
        {
           // transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
        }    
    }
    // allows to set target for the camera component
    public void setTarget(Transform target)
    {
        player = target;
    }
}
