using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV3 : MonoBehaviour, PlayerInputActions.IPlayerActions
{

    Animator animator;
    SpriteRenderer spriteRenderer;

    private float moveSpeed = 10f;
  
    public Rigidbody2D rb;
    public TrailRenderer tr;
    public PlayerStatValues playerStatValues;

    public float dashSpeed = 60f;
    public float flashAmount = 10f;

    private float trailVisibleTime = 0.2f;

    Vector2 moveDirection = Vector2.zero;
    Vector3 currentDirection;

  

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //setting the target of the camera to the player
        Camera.main.GetComponent<PlayerCamera>().setTarget(gameObject.transform);
    }
    void Update()
    {
        if(!IsPlayerMoving())
        {
            animator.SetBool("isMoving", false);
        } 
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        currentDirection = new Vector3(moveDirection.x, moveDirection.y).normalized;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        currentDirection = new Vector3(moveDirection.x, moveDirection.y).normalized;

        animator.SetBool("isMoving", true);

        if(currentDirection.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        if(currentDirection.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }
    public void OnFlash(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && playerStatValues.coinAmount > 0 && IsPlayerMoving() == true)
        {
            playerStatValues.coinAmount --;
            rb.MovePosition(transform.position + currentDirection * flashAmount);
        }  
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && playerStatValues.coinAmount > 0 && IsPlayerMoving() == true)
        {
            StartCoroutine(DashingMechanic());
        }      
    }
    private IEnumerator DashingMechanic()
    {
        playerStatValues.coinAmount --;
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        tr.emitting = true;
        yield return new WaitForSeconds(trailVisibleTime);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        tr.emitting = false;
    }

    private bool IsPlayerMoving()
    {
        bool result = false;
        if(rb.velocity.magnitude > 0)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }
}
