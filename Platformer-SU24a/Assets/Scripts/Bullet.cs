using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Configurable parameters
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int damageAmount = 1;

    // Private variables
    Vector2 fireDirection;

    // Cached references
    Rigidbody2D bulletRigidbody;

    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
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
        Destroy(this.gameObject);
    }
}