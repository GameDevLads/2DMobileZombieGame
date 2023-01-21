using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZombieMovement : SteeringBehavior
{
    public Rigidbody2D rb;
    public float speed;

    [SerializeField]
    float maxSeeAhead;

    Vector3 topLeft, topRight, bottomLeft, bottomRight;

    public Animator animator;
    
    Vector2 moveDirection = Vector2.zero;
    Vector3 currentDirection;

    SpriteRenderer spriteRenderer;
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         * ADD GAME OVER SCREEN HERE
         */ 
        if (collision.CompareTag("Player"))
        {

        }
    }

    public void CheckForCollisionDetected()
    {
        RaycastHit2D[] hit2D = new RaycastHit2D[2];

        /* 2 raycasts are used for this, one points from the bottom left corner to the top left corner of the agent and
        the other from the bottom right to the top right */
        hit2D[0] = Physics2D.Raycast(bottomLeft, topLeft - bottomLeft, maxSeeAhead, 1 << 9);
        hit2D[1] = Physics2D.Raycast(bottomRight, topRight - bottomRight, maxSeeAhead, 1 << 9);

        Vector3 dirOfMovementToAvoidObstacle;

        if (hit2D[0])
        {
            /* if a collision was detected on the left side of the bounding box, the direction of movement (to
            steer away from the obstacle) will be to the right. */
            dirOfMovementToAvoidObstacle = topRight - hit2D[0].collider.transform.position;

            /* Make the direction of vector to avoid obtacle, point away from it as much as possible to ensure the obstacle doesnt collide with it
             This can obviously be changed to make your own direction of movement when an obstacle is detected.*/
            dirOfMovementToAvoidObstacle *= Vector2.Distance(transform.position, hit2D[0].collider.transform.position);

            Steer(dirOfMovementToAvoidObstacle);

            Debug.DrawRay(hit2D[0].collider.transform.position, topRight - hit2D[0].collider.transform.position, Color.white);
        }
        else if (hit2D[1])
        {
            dirOfMovementToAvoidObstacle = topLeft - hit2D[1].collider.transform.position;
            dirOfMovementToAvoidObstacle *= Vector2.Distance(transform.position, hit2D[1].collider.transform.position);

            Steer(dirOfMovementToAvoidObstacle);

            Debug.DrawRay(hit2D[1].collider.transform.position, topLeft - hit2D[1].collider.transform.position, Color.white);
        }
        /* If no obstacle was detected, then just steer it towards it's current velocity */
        else Steer(location + (velocity.normalized * velocity.magnitude));
    }

    private void Update()
    {
        if(player != null)
        {
            
        }

        //dogshit
        if (startPosition.x != transform.position.x || startPosition.y != transform.position.y)
        {
            isMoving = true;
            animator.SetBool("isMoving", true);
        }



    }

}
