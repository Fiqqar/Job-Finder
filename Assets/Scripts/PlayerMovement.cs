using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Health health;
    public UICollect collect;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isKnocked = false;
    private float knockbackTimer = 0f;
    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isKnocked)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnocked = false;
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        var keyboard = Keyboard.current;
        moveInput = Vector2.zero;
        if (keyboard.wKey.isPressed) moveInput.y += 1;
        if (keyboard.sKey.isPressed) moveInput.y -= 1;
        if (keyboard.aKey.isPressed) moveInput.x -= 1;
        if (keyboard.dKey.isPressed) moveInput.x += 1;

        moveInput.Normalize();
    }

    void FixedUpdate()
    {
        if (!isKnocked)
            rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    public void Knockback(Vector2 direction, float force, float duration)
    {
        if (isDead) return;
        isKnocked = true;
        knockbackTimer = duration;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.Shake(0.05f, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coffee"))
        {
            health.RegenHealth(5);
            collect.AddCoffee();
        }
        else if (collision.gameObject.CompareTag("Job"))
        {
            collect.AddJob();
        }
    }
}
