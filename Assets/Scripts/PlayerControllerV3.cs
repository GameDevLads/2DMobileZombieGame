using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControllerV3 : MonoBehaviour, PlayerInputActions.IPlayerActions
{

    //Vector2 lastInput;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public float moveSpeed = 0f;

    public Rigidbody2D rb;
    public TrailRenderer tr;
    public FloatVariableSO coinAmountSO;

    [Tooltip("Player's default gun.")]
    public GameObject DefaultGun;

    [Tooltip("How far away the gun is from the player.")]
    public float GunPlayerDistance = 1.5f;

    public float dashSpeed = 60f;
    public float flashAmount = 10f;

    private float trailVisibleTime = 0.2f;
    private bool _fireGun = false;
    public Vector2 moveDirection = Vector2.zero;
    public Vector3 moveDirectionVector3;



    private GameObject _playerHandsGameObject;

    // This is the player hand object that has a gun as a child object which rotates around the player
    void InitPlayerHands()
    {
        _playerHandsGameObject = new GameObject("Player Hands");
        _playerHandsGameObject.AddComponent<PlayerHandsController>();
        var gun = Instantiate(DefaultGun, new Vector2(GunPlayerDistance, 0), Quaternion.identity);
        gun.transform.parent = _playerHandsGameObject.gameObject.transform;
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
        if (!IsPlayerMoving())
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
        var gunScripts = GetComponentsInChildren(typeof(IPlayerGun));

        foreach (var gunScript in gunScripts)
        {
            if (gunScript is IPlayerGun gun)
                gun.FireGun();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>().normalized;
        //creating a normalised vector will force the magnitude (length) of the vector to 1 (full speed). Meaning regardels how much the joystick is pushed the player will always move at the top speed. 
        moveDirectionVector3 = new Vector3(moveDirection.x, moveDirection.y);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        animator.SetBool("isMoving", true);

        if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnFlash(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && coinAmountSO.Value > 0 && IsPlayerMoving() == true)
        {
            coinAmountSO.Value--;
            rb.MovePosition(transform.position + moveDirectionVector3 * flashAmount);
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
        coinAmountSO.Value--;
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        tr.emitting = true;
        yield return new WaitForSeconds(trailVisibleTime);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        tr.emitting = false;
    }

    private bool IsPlayerMoving()
    {
        bool result = false;
        if (rb.velocity.magnitude > 0)
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
    }
}