using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] int pointForScorePickUp = 1;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player") && other.GetType() == typeof(CircleCollider2D))
        {
            gameManager.AddToScore(pointForScorePickUp);
            Destroy(gameObject);
        }
        
        
    }
}
