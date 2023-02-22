using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        private EnemyStats _enemyStats;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _reachDistance;
        [SerializeField]
        private int _currentWaypoint = 0;
        private List<Astar.Node> _path = new List<Astar.Node>();
        [SerializeField]
        private int _pathCount = 0;
        private Transform _target;
        private GameObject _player;
        private Vector3 _lastTargetPos;
        private Astar.Algorithm _algorithm;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        public bool TargetInRange = false;
        public bool IsMoving = false;
        private void Awake()
        {
            _enemyStats = GetComponent<EnemyStats>();
        }

        private void Start()
        {
            _speed = _enemyStats.Speed;
            _reachDistance = _enemyStats.AttackRange;
            _player = GameObject.FindWithTag("Player");
            _target = _player.transform;
            _lastTargetPos = _target.position;
            _algorithm = GetComponent<Astar.Algorithm>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }
        private void OnEnable()
        {
            _enemyStats.OnSpeedChanged.AddListener(UpdateSpeed);
        }
        private void OnDisable()
        {
            _enemyStats.OnSpeedChanged.RemoveListener(UpdateSpeed);
        }
        private void UpdateSpeed(float speed)
        {
            _speed = speed;
        }
        private void FixedUpdate()
        {
            if (_target == null)
                return;

            TargetInRange = Vector3.Distance(transform.position, _target.position) < _reachDistance;

            if (_target.position != _lastTargetPos)
            {
                _currentWaypoint = 0;
                _lastTargetPos = _target.position;
                findPath(transform.position, _target.position);
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

                    Vector3 dir = (_path[_currentWaypoint].worldPosition - transform.position);
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
            IsMoving = _animator.GetBool("isMoving");
        }

        public void findPath(Vector3 start, Vector3 end)
        {
            _path = _algorithm.FindPath(start, end);
        }
        private void OnDrawGizmos()
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