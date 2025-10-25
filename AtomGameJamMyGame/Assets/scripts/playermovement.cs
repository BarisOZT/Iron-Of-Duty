using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f; 
    private Rigidbody2D rb;
    private Vector2 movement;

    
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        
        if (movement != Vector2.zero)
        {
            animator.SetBool("isWalking", true);

            
            if (movement.x > 0)
                spriteRenderer.flipX = true;
            else if (movement.x < 0)
                spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
