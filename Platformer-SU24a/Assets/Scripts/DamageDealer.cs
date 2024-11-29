using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] int damageDealt = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damageDealt);
        }
        
    }
}
