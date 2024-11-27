using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // configurable parameters
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpStrength = 12f;
    [SerializeField] float coyoteTime = 0.5f;

    [Header("Ground Check")]
    [SerializeField] Transform feetTransform;
    [SerializeField] Vector2 groundCheck;
    [SerializeField] LayerMask groundLayer;

    [Header("Gravity")]
    [SerializeField] float extraGravity = 1200f;
    [SerializeField] float gravityDelay = 0.2f;
    [SerializeField] float maxFallSpeedVelocity = -20f;

    // private variables
    Vector2 movementVector;
    float timeInAir, coyoteTimer;
    bool doubleJumpAvailable;

    // cached references
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
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
        movementVector = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
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
        playerRigidbody.linearVelocityX = movementVector.x * moveSpeed;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.linearVelocityX) > Mathf.Epsilon;
        playerAnimator.SetBool("isRun", playerHasHorizontalSpeed);
    }

    void HandleSpriteFlip()
    {
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