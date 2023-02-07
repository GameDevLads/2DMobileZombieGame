using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public FloatVariableSO speed;
    public IntVariableSO reachDistance;
    private int currentWaypoint = 0;
    private List<Node> path = new List<Node>();
    public Transform target;
    private Vector3 lastTargetPos;
    private Vector3 lastEnemyPos;
    private Astar aStar;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        lastTargetPos = target.position;
        lastEnemyPos = transform.position;
        aStar = GetComponent<Astar>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
        
        if (path.Count > 0)
        {
            
            if (currentWaypoint <= path.Count - reachDistance.Value)
            {
                animator.SetBool("isMoving", true);
                Vector3 dir = (path[currentWaypoint].worldPosition - transform.position);
                dir = Vector3.ClampMagnitude(dir.normalized, 0.5f); 
                if (rb.velocity.normalized != (Vector2)dir)
                {
                    rb.velocity = new Vector2(dir.x * speed.Value, dir.y * speed.Value);
                }
                if (dir.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }

                if (Vector3.Distance(transform.position, path[currentWaypoint].worldPosition) < 0.1f) 
                {
                    currentWaypoint++;
                }
            }
            else
            {
                animator.SetBool("isMoving", false);
                rb.velocity = Vector2.zero;
            }
            if(currentWaypoint > path.Count)
            {
                animator.SetBool("isMoving", false);
                currentWaypoint = 0;
            }

        }
        else
        {
            animator.SetBool("isMoving", false);
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