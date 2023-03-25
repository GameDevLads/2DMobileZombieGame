using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControllerV3 : MonoBehaviour, PlayerInputActions.IPlayerActions
{

    private Vector2 _moveDirection = Vector2.zero;
    private Vector2 _pointerPosition;
    private Vector3 _moveDirectionVector3;

    Animator animator;
    SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;
    public TrailRenderer tr;
    public FloatVariableSO coinAmountSO;

    [Tooltip("Player's default weapon.")]
    public GameObject DefaultWeapon;

    [Tooltip("How far away the weapon is from the player.")]
    public float WeaponPlayerDistance = 1.5f;

    [SerializeField]
    private float _dashSpeed = 60f;

    [SerializeField]
    private float _flashAmount = 10f;

    [SerializeField]
    private float _moveSpeed = 10f;

    private float _trailVisibleTime = 0.2f;
    private bool _useWeapon = false;
    private bool _isGamepad;
    private bool _manualAim = false;

    private GameObject _playerHandsGameObject;

    private GameObject _enemy;

    // This is the player hand object that has a weapon as a child object which rotates around the player
    void InitPlayerHands()
    {
        _playerHandsGameObject = new GameObject("Player Hands");
        _playerHandsGameObject.AddComponent<PlayerHandsController>();
        var weapon = Instantiate(DefaultWeapon, new Vector2(WeaponPlayerDistance, 0), Quaternion.identity);
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

        _enemy = GameObject.FindWithTag("Enemy");
    }

    void Update()
    {
        if (!IsPlayerMoving())
        {
            animator.SetBool("isMoving", false);
        }

        if (_useWeapon)
        {
            UseWeapon();
        }
        
        //direction is the position of the enemy - the position of the player
        Vector3 direction = _enemy.transform.position - transform.position;
        if(_manualAim == false)
        {
            AutoAim(direction);
        }
        
    }

    private void UseWeapon()
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
        _moveDirection = context.ReadValue<Vector2>().normalized;
        //creating a normalised vector will force the magnitude (length) of the vector to 1 (full speed). Meaning regardels how much the joystick is pushed the player will always move at the top speed. 
        _moveDirectionVector3 = new Vector3(_moveDirection.x, _moveDirection.y);
        rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);
        animator.SetBool("isMoving", true);

        if (_moveDirection.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        if (_moveDirection.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void Flash(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && coinAmountSO.Value > 0 && IsPlayerMoving() == true)
        {
            coinAmountSO.Value--;
            rb.MovePosition(transform.position + _moveDirectionVector3 * _flashAmount);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && coinAmountSO.Value > 0 && IsPlayerMoving() == true)
        {
            StartCoroutine(DashingMechanic());
        }
    }

    private IEnumerator DashingMechanic()
    {
        coinAmountSO.Value--;
        rb.velocity = new Vector2(_moveDirection.x * _dashSpeed, _moveDirection.y * _dashSpeed);
        tr.emitting = true;
        yield return new WaitForSeconds(_trailVisibleTime);
        rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);
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

    public void OnUseWeapon(InputAction.CallbackContext context)
    {
        // We need to check when the mouse click starts and ends as there is no other way of continuously firing when the mouse is held
        if (context.started)
            _useWeapon = true;
        else if (context.canceled)
            _useWeapon = false;
    }

    public void OnAimWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _manualAim = true;
        }
        else if (context.canceled)
        {
            _manualAim = false;
        }
       
        if (_isGamepad == false)
        {
            // Update the rotation of the weapon based on the pointer position
            var pointerPosition = context.ReadValue<Vector2>();
            var playerHandsWeapon = _playerHandsGameObject.GetComponent<PlayerHandsController>();
            playerHandsWeapon.UpdatePositionMouse(pointerPosition);
        }

        else if (_isGamepad == true)
        {
            var pointerPosition = context.ReadValue<Vector2>();
            var playerHandsWeapon = _playerHandsGameObject.GetComponent<PlayerHandsController>();
            playerHandsWeapon.UpdatePositionGamepadAndAutoAim(pointerPosition);
        }
    }

    public void AutoAim(Vector3 enemyPosition)
    {
        var pointerPosition = enemyPosition;
        var playerHandsWeapon = _playerHandsGameObject.GetComponent<PlayerHandsController>();
        playerHandsWeapon.UpdatePositionGamepadAndAutoAim(pointerPosition);
    }

    public void OnDeviceChange(PlayerInput playerInput)
    {
        _isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        Dash(context);
    }
    public void OnAbility2(InputAction.CallbackContext context)
    {
        Flash(context);
    }
    public void OnAbility3(InputAction.CallbackContext context)
    {
        Debug.Log("Ability 3 Place Holder");
    }
}