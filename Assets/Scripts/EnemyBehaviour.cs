using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 3.0f; // Speed of the enemy movement
    private Transform player; // Reference to the enemy's Transform component
    private Rigidbody2D rb; // Reference to the enemy's Rigidbody2D component
    public int damage = 10; // Damage dealt to the player on collision

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop moving if the player is not found
        }
    }

    // Destroy enemy when colliding with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}