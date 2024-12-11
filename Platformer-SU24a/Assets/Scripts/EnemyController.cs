using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // configurable parameters
    [Header("Enemy Stats")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int damageAmount = 1;
    [SerializeField] float knockbackThrust = 5f;

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
    Knockback knockback;

    void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        knockback = GetComponent<Knockback>();
    }

    void FixedUpdate()
    {
        if (!knockback.GetIsKnockedBack())
        {
            Move();
            LedgeCheck();
        }
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(damageAmount);

        Knockback knockback = other.gameObject.GetComponent<Knockback>();
        knockback?.GetKnockedBack(transform.position, knockbackThrust);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, groundCheck);
    }
}