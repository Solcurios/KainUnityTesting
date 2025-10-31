using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public float detectionRadius = 20f;
    public float moveSpeed = 3f;
    public int damage = 10;
    public float attackCooldown = 1f;

    private Transform player;
    private float nextAttackTime = 0f;
    private Rigidbody rb;
    private Animator animator;

    private bool isChasing = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (player == null)
        {
            SetIdle();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            ChasePlayer();
        }
        else
        {
            SetIdle();
        }
    }

    void ChasePlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 moveDir = (targetPos - transform.position).normalized;

        rb.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);

        if (moveDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDir);

        // ✅ Play running animation
        animator.SetBool("isRunning", true);
        animator.SetBool("isJumping", false);
        isChasing = true;
    }

    void SetIdle()
    {
        if (isChasing)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            isChasing = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    // Example jump trigger
    public void Jump()
    {
        // Play jump animation (optional trigger)
        animator.SetBool("isJumping", true);
        animator.SetBool("isRunning", false);

        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
