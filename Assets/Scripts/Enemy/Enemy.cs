using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if(currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Enemy died!");
    }
}
