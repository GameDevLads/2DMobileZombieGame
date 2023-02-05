using Assets.Scripts.Interfaces;
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
    public IntVariableSO coinAmountSO;

    [Tooltip("Player's default weapon.")]
    public GameObject DefaultWeapon;

    [Tooltip("How far away the gun is from the player.")]
    public float GunPlayerDistance = 1.5f;

    public float dashSpeed = 60f;
    public float flashAmount = 10f;

    private float trailVisibleTime = 0.2f;
    private bool _fireGun = false;

    private Vector2 _pointerPosition;
    Vector2 moveDirection = Vector2.zero;
    Vector3 currentDirection;

    private GameObject _playerHandsGameObject;

    // This is the player hand object that has a gun as a child object which rotates around the player
    void InitPlayerHands()
    {
        _playerHandsGameObject = new GameObject("Player Hands");
        _playerHandsGameObject.AddComponent<PlayerHandsController>();
        var weapon = Instantiate(DefaultWeapon, new Vector2(GunPlayerDistance, 0), Quaternion.identity);
        weapon.transform.parent = _playerHandsGameObject.gameObject.transform;
        _playerHandsGameObject.transform.parent = gameObject.transform;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitPlayerHands();

        //setting the target of the camera to the player
        Camera.main.GetComponent<PlayerCamera>().setTarget(gameObject.transform);
    }

    void Update()
    {
        if(!IsPlayerMoving())
        {
            animator.SetBool("isMoving", false);
        } 

        if (_fireGun)
        {
            FireGun();
        }
    }

    private void FireGun()
    {
        var weaponScripts = GetComponentsInChildren(typeof(IPlayerWeapon));

        foreach (var weaponScript in weaponScripts)
        {
            if (weaponScript is IPlayerWeapon weapon)
            {
                weapon.UseWeapon(_pointerPosition);
            }   
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
        if (context.ReadValueAsButton() && coinAmountSO.Value > 0 && IsPlayerMoving() == true)
        {
            coinAmountSO.Value --;
            rb.MovePosition(transform.position + currentDirection * flashAmount);
        }  
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && coinAmountSO.Value > 0 && IsPlayerMoving() == true)
        {
            StartCoroutine(DashingMechanic());
        }      
    }

    private IEnumerator DashingMechanic()
    {
        coinAmountSO.Value --;
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

    public void OnFireGun(InputAction.CallbackContext context)
    {
        // We need to check when the mouse click starts and ends as there is no other way of continuously firing when the mouse is held
        if (context.started)
            _fireGun = true;
        else if (context.canceled)
            _fireGun = false;
    }

    public void OnAimGun(InputAction.CallbackContext context)
    {
        // Update the rotation of the gun based on the pointer position
        var pointerPosition = context.ReadValue<Vector2>();
        var playerHandsGun = _playerHandsGameObject.GetComponent<PlayerHandsController>();
        playerHandsGun.UpdatePosition(pointerPosition);

        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, Camera.main.nearClipPlane));
        _pointerPosition = mouseWorldPosition;
    }
}