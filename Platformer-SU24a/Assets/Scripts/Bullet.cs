using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Configurable parameters
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int damageAmount = 1;
    [SerializeField] float knockbackThrust = 20f;

    // Private variables
    Vector2 fireDirection;

    // Cached references
    Rigidbody2D bulletRigidbody;
    PlayerController playerController;

    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    void FixedUpdate()
    {
        bulletRigidbody.linearVelocity = fireDirection * moveSpeed;
    }

    public void Init(Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        fireDirection = (mousePos - bulletSpawnPos).normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(damageAmount);

        Knockback knockback = other.gameObject.GetComponent<Knockback>();
        knockback?.GetKnockedBack(playerController.transform.position, knockbackThrust);

        Destroy(this.gameObject);
    }
}