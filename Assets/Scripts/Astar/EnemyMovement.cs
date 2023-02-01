using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 3f;
    private int currentWaypoint = 0;
    private List<Node> path = new List<Node>();
    public Transform target;
    private Vector3 lastTargetPos;
    private Vector3 lastEnemyPos;
    private Astar aStar;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        lastTargetPos = target.position;
        lastEnemyPos = transform.position;
        aStar = GetComponent<Astar>();
    }
    void Update()
    {
        if(target == null)
        {
            animator.SetBool("isMoving", false);
            return;
        }
        if(target.position != lastTargetPos ){
            lastTargetPos = target.position;
            lastEnemyPos = transform.position;
            findPath(transform.position, target.position);
        }
        
        // Move the enemy
        if (path.Count > 0)
        {
            if (currentWaypoint < path.Count)
            {
                animator.SetBool("isMoving", true);
                Vector3 dir = (path[currentWaypoint].worldPosition - transform.position).normalized;
                if(dir.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                if (dir.x < 0)
                {
                    spriteRenderer.flipX = false;
                }
                // Allow the enemy to move diagonally
                if (dir.x != 0 && dir.y != 0)
                {
                    dir *= 0.8f;
                }
                transform.Translate(dir * speed * Time.deltaTime);//needs changing, do not use transform to move things around
                if (Vector3.Distance(transform.position, path[currentWaypoint].worldPosition) < 0.1f)
                {
                    currentWaypoint++;
                }
            }
            if(currentWaypoint > path.Count)
            {
                currentWaypoint = 0;
            }

        }
    }

    public void findPath(Vector3 start, Vector3 end)
    {
        path = aStar.FindPath(start,end);
    }
    void OnDrawGizmos()
    {
        // Draw the path
        if (path.Count > 0)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.color = Color.black;
                Vector3 cur = path[i].worldPosition;
                if(path.Count > i + 1)
                {
                    Vector3 next = path[i + 1].worldPosition;
                    Gizmos.DrawLine(cur, next);
                }

            }
        }
    }

    
}