using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    // Add these:
    public GameObject xpOrbPrefab;  
    public int xpOrbCount = 3;      
    public float dropRadius = 1f;   

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        DropXPOrbs();
        Destroy(gameObject);
    }

   void DropXPOrbs()
{
    for (int i = 0; i < xpOrbCount; i++)
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * dropRadius;
        randomPos.y = transform.position.y + 1f;  
        Instantiate(xpOrbPrefab, randomPos, Quaternion.identity);
    }
}

}
