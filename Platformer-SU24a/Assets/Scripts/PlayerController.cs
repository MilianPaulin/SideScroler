using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // configurable parameters
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpStrength = 7f;
    [SerializeField] float coyoteTime = 0.5f;

    [Header("Ground Check")]
    [SerializeField] Transform feetTransform;
    [SerializeField] Vector2 groundCheck;
    [SerializeField] LayerMask groundLayer;

    [Header("Gravity")]
    [SerializeField] float extraGravity = 700f;
    [SerializeField] float gravityDelay = 0.2f;
    [SerializeField] float maxFallSpeedVelocity = -20f;

    // private variables
    Vector2 movementVector;
    float timeInAir, coyoteTimer;
    bool doubleJumpAvailable;

    // cached references
    Rigidbody2D playerRigidbody;
    Health health;
    Animator playerAnimator;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CoyoteTimer();
        GravityDelay();
    }

    void FixedUpdate()
    {
        Move();
        ExtraGravity();
        HandleSpriteFlip();
        CheckJumpAnimation();
    }

    void OnMove(InputValue value)
    {
        if (health.GetIsDead()) { return; }

        movementVector = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (health.GetIsDead()) { return; }

        if (value.isPressed)
        {
            if (CheckGrounded())
            {
                ApplyJumpForce();
            }
            else if (coyoteTimer > 0f)
            {
                ApplyJumpForce();
            }
            else if (doubleJumpAvailable)
            {
                doubleJumpAvailable = false;
                ApplyJumpForce();
            }
        }
    }

    void ApplyJumpForce()
    {
        playerAnimator.SetBool("isJump", true);

        playerRigidbody.linearVelocity = Vector2.zero;
        timeInAir = 0f;
        coyoteTimer = 0f;
        playerRigidbody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }

    void GravityDelay()
    {
        if (!CheckGrounded())
        {
            timeInAir += Time.deltaTime;
        }
        else
        {
            timeInAir = 0f;
        }
    }

    void ExtraGravity()
    {
        if (timeInAir > gravityDelay)
        {
            playerRigidbody.AddForce(new Vector2(0f, -extraGravity * Time.deltaTime));

            if (playerRigidbody.linearVelocityY < maxFallSpeedVelocity)
            {
                playerRigidbody.linearVelocityY = maxFallSpeedVelocity;
            }
        }
    }

    void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            coyoteTimer = coyoteTime;
            doubleJumpAvailable = true;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(
            feetTransform.position, groundCheck, 0f, groundLayer);

        return isGrounded;
    }

    void Move()
    {
        if (health.GetIsDead())
        {
            playerRigidbody.linearVelocityX = 0f;
            return;
        }

        playerRigidbody.linearVelocityX = movementVector.x * moveSpeed;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.linearVelocityX) > Mathf.Epsilon;
        playerAnimator.SetBool("isRun", playerHasHorizontalSpeed);
    }

    void HandleSpriteFlip()
    {
        if (health.GetIsDead()) { return; }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            // Turn left
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            // Turn right
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    void CheckJumpAnimation()
    {
        bool isJumping = !CheckGrounded() && Mathf.Abs(playerRigidbody.linearVelocityY) > Mathf.Epsilon;
        playerAnimator.SetBool("isJump", isJumping);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetTransform.position, groundCheck);
    }
}