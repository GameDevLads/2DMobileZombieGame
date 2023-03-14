using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Stats;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public int reachDistance;
    [SerializeField]
    private int currentWaypoint = 0;
    private List<Node> path = new();
    [SerializeField]
    private int pathCount = 0;
    public Transform target;
    private Vector3 lastTargetPos;
    private Astar aStar;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private EnemyStats enemyStats;

    void Start()
    {
        lastTargetPos = target.position;
        aStar = GetComponent<Astar>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        enemyStats = GetComponent<EnemyStats>();
        speed = enemyStats.MovementSpeed;
        reachDistance = (int)enemyStats.AttackRange;
    }
    void FixedUpdate()
    {
        if(target == null)
            return;

        if(target.position != lastTargetPos)
        {
            currentWaypoint = 0;
            lastTargetPos = target.position;
            FindPath(transform.position, target.position);
        }
        
        animator.SetBool("isMoving", false);
        rb.velocity = Vector2.zero;

        if (path.Count > 0)
        {
            // For debugging
            pathCount = path.Count;

            if (currentWaypoint <= path.Count - reachDistance)
            {
                animator.SetBool("isMoving", true);

                Vector3 dir = path[currentWaypoint].worldPosition - transform.position;
                dir = Vector3.ClampMagnitude(dir.normalized, 0.5f); 

                if (rb.velocity.normalized != (Vector2)dir)
                    rb.velocity = new Vector2(dir.x * speed, dir.y * speed);

                if (dir.x > 0)
                    spriteRenderer.flipX = true;
                else
                    spriteRenderer.flipX = false;

                if (Vector3.Distance(transform.position, path[currentWaypoint].worldPosition) < 0.1f) 
                    currentWaypoint++;
            }
            if(currentWaypoint > path.Count)
            {
                animator.SetBool("isMoving", false);
                rb.velocity = Vector2.zero;
                currentWaypoint = 0;
            }
        }
    }

    public void FindPath(Vector3 start, Vector3 end)
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