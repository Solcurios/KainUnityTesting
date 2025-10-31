using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Call this to reduce player health
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Clamp to 0 so it never goes negative
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Player took " + amount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // Add death behavior here, e.g. disable movement, play animation, restart level
        // Example: disable the player controller
        var controller = GetComponent<MonoBehaviour>(); // replace with your FirstPersonController reference if needed
        if (controller != null)
            controller.enabled = false;
    }
}
