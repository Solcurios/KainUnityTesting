using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 10;
    public float pickupRange = 3f;
    public float moveSpeed = 5f;
    private Transform player;

    void Start() => player = GameObject.FindWithTag("Player").transform;

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < pickupRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerXP>().AddXP(xpAmount);
            Destroy(gameObject);
        }
    }
}
