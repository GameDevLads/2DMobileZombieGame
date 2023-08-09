using Assets.Scripts.AStar;
using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementV2 : MonoBehaviour
{
    public StatsSO StatsSO;
    private float _speed;
    private GameObject _player;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private float distance; 

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _speed = StatsSO.CurrentStats.MovementSpeed;
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        _animator.SetBool("isMoving", true);

        distance = Vector2.Distance(transform.position, _player.transform.position);
        Vector2 direction = _player.transform.position - transform.position;
        
        transform.position = Vector2.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);

        if (direction.x > 0)
        {
            _spriteRenderer.flipX = true;
        }

        else
        {
            _spriteRenderer.flipX = false;
        }

        if (_rb.IsSleeping())
        {
            _animator.SetBool("isMoving", false);
            _rb.velocity = Vector2.zero;
        }
    }
}
