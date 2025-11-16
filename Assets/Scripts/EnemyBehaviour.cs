using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public int damage = 10;
    public float knockbackForce = 20f;
    public float knockbackDuration = 0.2f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            PlayerMovement playerMove = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerHealth != null)
                playerHealth.TakeDamage(damage);

            if (playerMove != null)
            {
                Vector2 knockDir = (collision.transform.position - transform.position).normalized;
                playerMove.Knockback(knockDir, knockbackForce, knockbackDuration);
            }

            Destroy(gameObject);
        }
    }
}
