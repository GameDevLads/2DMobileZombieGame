using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public Vector3VariableSO enemyPositionFromPlayerSO;
    public FloatVariableSO autoAimRangeSO;

    private OccludableCollider occludableCollider;
    private AutoAimCollider autoAimCollider;

    /// <summary>
    /// This is the player hand object that has a weapon as a child object which rotates around the player
    /// </summary>
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
        occludableCollider = transform.Find("OccludableCollider").GetComponent<OccludableCollider>();

        occludableCollider.OnTriggerEnter2D_Action += OccludableCollider_OnCollisionEnter2D;
        occludableCollider.OnTriggerExit2D_Action += OccludableCollider_OnCollisionExit2D;

        autoAimCollider = transform.Find("AutoAimCollider").GetComponent<AutoAimCollider>();
        autoAimCollider.OnTriggerStay2D_Action += AutoAimCollider_OnTriggerStay2D;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitPlayerHands();

        //setting the target of the camera to the player
        Camera.main.GetComponent<PlayerCamera>().setTarget(gameObject.transform);

        //sets the radius of the AutoAim Colider to the value of autoAimtRandeSO. This value will be modified by each of the guns, meaning each gun will have a different autoaim range. At some point i should do this in the update method to make it more dynamic by having the value change mid game. 

        autoAimCollider.cr.radius = autoAimRangeSO.Value;
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
        //_enemyPositionFromPlayerSO is the Vector3 position of the enemy in relation to the player.
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            enemyPositionFromPlayerSO.Value = FindClosestEnemy().transform.position - transform.position;
        }
    }

    /// <summary>
    /// The circular Collider stays trigged as long as an enemy is inside and the player isn't manually aiming.
    /// the trigger, makes the gun shoot and engages the auto aim.
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && _manualAim == false)
        {
            UseWeapon();
            AutoAim(enemyPositionFromPlayerSO.Value);
        }
    }

    private void AutoAimCollider_OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && _manualAim == false)
        {
            UseWeapon();
            AutoAim(enemyPositionFromPlayerSO.Value);
        }
    }

    /// <summary>
    /// Finds the enemy object closest to the player and returns it. 
    /// </summary>
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
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
    /// <summary>
    /// When the below methods receive trigger actions on enter and on exit from the OccludableColliders child class that this script subscribes to, they make the object that is being colided with transparant. 
    /// </summary>
    private void OccludableCollider_OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Occludable") && collision.GetType() == typeof(PolygonCollider2D))
        {
            SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                spriteRenderer.sortingOrder = 12;
            }
        }

        if(collision.CompareTag("SetSortLayerForeground") && collision.GetType() == typeof(PolygonCollider2D))
        {
            SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
            SpriteRenderer[] children = collision.GetComponentsInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Default";
            }

            if (children != null)
            {
                children[1].sortingLayerName = "Foreground";
                children[2].sortingLayerName = "Foreground";
            }
        }

        if(collision.CompareTag("SlowWalking"))
        {
            ///this still has issues with the colliders, I think there are too many bushes next to one another with individual colliders and that's what causes problems
            ///when walking through them it still does that thing where you suddenly move fast through them when you shouldn't
            ///i.e it triggers the collision exit method and doesn't trigger the collision enter right away
            ///so thats why its commented out for now, if you lot know how to do it fix it please
          //  _moveSpeed = 2f;
        }
    }
    private void OccludableCollider_OnCollisionExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Occludable"))
        {
            SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                spriteRenderer.sortingOrder = 0;
            }
        }
        if (collision.CompareTag("SetSortLayerForeground"))
        {
            SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
            SpriteRenderer[] children = collision.GetComponentsInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Background";
            }
            if (children != null)
            {
                children[1].sortingLayerName = "Default";
                children[2].sortingLayerName = "Default";
            }
        }
        if (collision.CompareTag("SlowWalking"))
        {
            _moveSpeed = 10f;
        }
    }

}