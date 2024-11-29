using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Script : MonoBehaviour
{
    [SerializeField] int healthPoints = 1;

    [SerializeField] bool isDead = false;


    public void TakeDamage(int damage)
    {
        healthPoints = damage;

        if (healthPoints <= 0)
        {
            isDead = true;
        }
    }

    public bool GetIsDead()
    {
        return isDead;
    }

}
