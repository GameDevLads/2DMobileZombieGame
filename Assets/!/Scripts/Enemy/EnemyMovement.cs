using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Stats;
using Assets.Scripts.AStar;

namespace Assets.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        public StatsSO StatsSO;
        private float _speed;
        private float _reachDistance;
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

        void Start()
        {
            _player = GameObject.FindWithTag("Player");
            Target = _player.transform;
            _lastTargetPos = Target.position;
            _aStar = GetComponent<Algorithm>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _speed = StatsSO.CurrentStats.MovementSpeed;
            _reachDistance = StatsSO.CurrentStats.AttackRange;
        }
        void FixedUpdate()
        {
            if (Target == null)
                return;

            if (Target.position != _lastTargetPos)
            {
                _currentWaypoint = 0;
                _lastTargetPos = Target.position;
                FindPath(transform.position, Target.position);
            }

            _animator.SetBool("isMoving", false);
            _rb.velocity = Vector2.zero;

            if (_path.Count > 0)
            {
                // For debugging
                _pathCount = _path.Count;

                if (_currentWaypoint <= _path.Count - _reachDistance)
                {
                    _animator.SetBool("isMoving", true);

                    Vector3 dir = _path[_currentWaypoint].worldPosition - transform.position;
                    dir = Vector3.ClampMagnitude(dir.normalized, 0.5f);

                    if (_rb.velocity.normalized != (Vector2)dir)
                        _rb.velocity = new Vector2(dir.x * _speed, dir.y * _speed);

                    if (dir.x > 0)
                        _spriteRenderer.flipX = true;
                    else
                        _spriteRenderer.flipX = false;

                    if (Vector3.Distance(transform.position, _path[_currentWaypoint].worldPosition) < 0.1f)
                        _currentWaypoint++;
                }
                if (_currentWaypoint > _path.Count)
                {
                    _animator.SetBool("isMoving", false);
                    _rb.velocity = Vector2.zero;
                    _currentWaypoint = 0;
                }
            }
        }

        public void FindPath(Vector3 start, Vector3 end)
        {
            _path = _aStar.FindPath(start, end);
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