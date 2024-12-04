using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Configurable parameters
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] int healthPoints = 1;
    [SerializeField] bool isPlayer = false;

    // Private variables
    bool isDead = false;

    void Awake()
    {
        if (isPlayer && livesText != null)
        {
            livesText.text = "Lives: " + healthPoints.ToString();
        }
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (isPlayer && livesText != null)
        {
            livesText.text = "Lives: " + healthPoints.ToString();
        }

        if (healthPoints <= 0)
        {
            isDead = true;

            if (!isPlayer)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}