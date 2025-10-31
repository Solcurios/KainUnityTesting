using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 30f;
    public float spawnInterval = 5f;
    private float nextSpawnTime = 0f;

    public Transform player;
    public Camera playerCamera;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            Vector3 spawnPos = GetSpawnPositionOutsideFOV();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    Vector3 GetSpawnPositionOutsideFOV()
    {
        Vector3 spawnPos = Vector3.zero;
        bool validPos = false;

        float halfFOV = playerCamera.fieldOfView / 2f;
        int maxAttempts = 30;
        int attempts = 0;

        while (!validPos && attempts < maxAttempts)
        {
            attempts++;

            Vector2 randomDir2D = Random.insideUnitCircle.normalized;
            Vector3 randomDir = new Vector3(randomDir2D.x, 0, randomDir2D.y);

            float angle = Vector3.Angle(playerCamera.transform.forward, randomDir);

            if (angle > halfFOV + 10f)
            {
                Vector3 candidatePos = player.position + randomDir * spawnRadius + Vector3.up * 50f;

                RaycastHit hit;
                if (Physics.Raycast(candidatePos, Vector3.down, out hit, 100f, LayerMask.GetMask("Floor")))
                {
                    // Calculate height offset based on prefab's local scale
                    float heightOffset = enemyPrefab.transform.localScale.y / 2f;

                    spawnPos = hit.point + Vector3.up * heightOffset;
                    validPos = true;
                }
            }
        }

        // Fallback if no valid spawn found
        if (!validPos)
        {
            spawnPos = player.position + player.transform.forward * spawnRadius;
            spawnPos.y = player.position.y + enemyPrefab.transform.localScale.y / 2f;
        }

        return spawnPos;
    }
}
