using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    // Configurable parameters
    [SerializeField] float knockbackTime = 0.2f;

    // Private variables
    Vector3 hitDirection;
    float knockbackThrust;
    bool gettingKnockedback = false;

    // Cached references
    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Vector3 hitDirection, float knockbackThrust)
    {
        Debug.Log(gameObject.transform.name + " got knocked back!");
        this.hitDirection = hitDirection;
        this.knockbackThrust = knockbackThrust;
        gettingKnockedback = true;
        ApplyKnockbackForce();
    }

    void ApplyKnockbackForce()
    {
        Vector3 difference = (transform.position - hitDirection).normalized
                                * knockbackThrust
                                * rigidBody.mass;

        rigidBody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockbackTime);
        StopKnockRoutine();
    }

    void StopKnockRoutine()
    {
        rigidBody.linearVelocity = Vector2.zero;
        gettingKnockedback = false;
    }

    public bool GetGettingKnockedback()
    {
        return gettingKnockedback;
    }
}