using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Stats;
using Assets.Scripts.AStar;

namespace Assets.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        private float _speed;
        private int _reachDistance;
        [SerializeField]
        private int _currentWaypoint = 0;
        private List<AStarNode> _path = new();
        [SerializeField]
        private int _pathCount = 0;
        private GameObject _player;
        [HideInInspector]
        public Transform Target;
        private Vector3 _lastTargetPos;
        private Algorithm _aStar;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        private EnemyStats _enemyStats;
        private Vector2 _knockbackVelocity;

        void Start()
        {
            _player = GameObject.FindWithTag("Player");
            Target = _player.transform;
            _lastTargetPos = Target.position;
            _aStar = GetComponent<Algorithm>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _enemyStats = GetComponent<EnemyStats>();
            _speed = _enemyStats.MovementSpeed;
            _reachDistance = (int)_enemyStats.AttackRange;
        }
        void FixedUpdate()
        {
            if (Target == null)
                return;

            // Check if the target's position has changed since the last frame
            if (Target.position != _lastTargetPos)
            {
                // Reset the current waypoint index since the path has changed
                _currentWaypoint = 0;

                _lastTargetPos = Target.position;

                FindPath(transform.position, Target.position);
            }

            _animator.SetBool("isMoving", false);

            ReduceKnockbackForce(); // Reduce the knockback velocity on every frame so that the enemy doesn't get stuck in a knockback state

            // If there are waypoints in the path to follow
            if (_path.Count > 0)
            {
                _pathCount = _path.Count;

                // If the enemy hasn't reached the target (current waypoint is far from the end of the path)
                if (_currentWaypoint < _path.Count - _reachDistance)
                {
                    _animator.SetBool("isMoving", true);

                    // Calculate the direction to the current waypoint
                    Vector3 dir = _path[_currentWaypoint].worldPosition - transform.position;

                    // Normalize the direction and limit the maximum length to 0.5
                    dir = Vector3.ClampMagnitude(dir.normalized, 0.5f);

                    // Calculate the velocity required to move in the direction of the waypoint and apply any knockback force that might be acting on the enemy
                    _rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed) + _knockbackVelocity;

                    if (dir.x > 0)
                        _spriteRenderer.flipX = true;
                    else
                        _spriteRenderer.flipX = false;

                    // If the entity is close enough to the waypoint, increment the waypoint counter to move to the next one
                    if (Vector3.Distance(transform.position, _path[_currentWaypoint].worldPosition) < 0.1f)
                        _currentWaypoint++;
                }

                // If we've reached the end of the path, stop moving
                if (_currentWaypoint >= _path.Count)
                {
                    _animator.SetBool("isMoving", false);
                    _rb.velocity = Vector2.zero;
                }
            }
            // If there's no path, stop moving
            else
            {
                _animator.SetBool("isMoving", false);
                _rb.velocity = Vector2.zero;
            }
        }


        public void FindPath(Vector3 start, Vector3 end)
        {
            _path = _aStar.FindPath(start, end);
        }
        /// <summary>
        /// Applies a knockback force to the enemy.
        /// </summary>
        public void Knockback(Vector2 direction, float force)
        {
            _knockbackVelocity = direction * force;
        }

        /// <summary>
        /// Reduces the knockback force over time.
        /// </summary>
        private void ReduceKnockbackForce()
        {
            float knockbackDampening = 5f; // Adjust this value to control how quickly the knockback force is reduced.
            _knockbackVelocity = Vector2.Lerp(_knockbackVelocity, Vector2.zero, Time.fixedDeltaTime * knockbackDampening);
        }

        void OnDrawGizmos()
        {
            // Draw the path
            if (_path.Count > 0)
            {
                for (int i = 0; i < _path.Count; i++)
                {
                    Gizmos.color = Color.black;
                    Vector3 cur = _path[i].worldPosition;
                    if (_path.Count > i + 1)
                    {
                        Vector3 next = _path[i + 1].worldPosition;
                        Gizmos.DrawLine(cur, next);
                    }

                }
            }
        }


    }
}