using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // configurable parameters
    [Header("Enemy Stats")]
    [SerializeField] float moveSpeed = 1f;

    [Header("Ground & Ledge Detection")]
    [SerializeField] Transform ledgeCheckPosition;
    [SerializeField] float ledgeCheckLength;
    [SerializeField] float groundCheck = 1f;
    [SerializeField] LayerMask groundLayer;

    // private variables
    bool isFacingRight;

    // cached references
    Rigidbody2D enemyRigidbody;
    Health health;

    void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    void FixedUpdate()
    {
        Move();
        LedgeCheck();
    }

    void Move()
    {
        if (health.GetIsDead())
        {
            enemyRigidbody.linearVelocityX = 0f;
            return;
        }

        if (CheckGrounded())
        {
            enemyRigidbody.linearVelocityX = transform.right.x * moveSpeed;
        }
        else
        {
            enemyRigidbody.linearVelocityX = 0f;
        }
    }

    void LedgeCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            ledgeCheckPosition.position,
            Vector2.down,
            ledgeCheckLength,
            groundLayer);

        if (hit.collider == null && CheckGrounded())
        {
            isFacingRight = !isFacingRight;

            if (isFacingRight)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
            }
        }
    }

    bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapCircle(transform.position, groundCheck, groundLayer);

        return isGrounded;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, groundCheck);
    }
}