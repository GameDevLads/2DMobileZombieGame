using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats enemyStats;
    [SerializeField]
    private float speed => enemyStats.speed;
    [SerializeField]
    private float reachDistance => enemyStats.attackRange;
    [SerializeField]
    private int currentWaypoint = 0;
    private List<Node> path = new List<Node>();
    [SerializeField]
    private int pathCount = 0;
    Transform target;
    GameObject player;
    private Vector3 lastTargetPos;
    private Astar aStar;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public bool targetInRange = false;
    public bool isMoving = false;

    void Start()
    {
        enemyStats = GetComponent<Assets.Scripts.Enemy>().EnemyStats;
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        lastTargetPos = target.position;
        aStar = GetComponent<Astar>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if(target == null)
            return;
        
        targetInRange = Vector3.Distance(transform.position, target.position) < reachDistance;

        if(target.position != lastTargetPos)
        {
            currentWaypoint = 0;
            lastTargetPos = target.position;
            findPath(transform.position, target.position);
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

                Vector3 dir = (path[currentWaypoint].worldPosition - transform.position);
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
        isMoving = animator.GetBool("isMoving");
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