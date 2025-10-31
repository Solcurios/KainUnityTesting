using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.5f; // Seconds between shots
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab;

    private float nextTimeToFire = 0f;

    // Reference to the Input Action for firing
    public InputAction fireAction;

    private void OnEnable()
    {
        fireAction.Enable();
    }

    private void OnDisable()
    {
        fireAction.Disable();
    }

    void Update()
    {
        // Use fireAction.IsPressed() to check if fire button is held down
        if (fireAction.IsPressed() && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null) muzzleFlash.Play();

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range))
        {
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (hitEffectPrefab != null)
            {
                GameObject impactGO = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 1f);
            }
        }
    }
}
