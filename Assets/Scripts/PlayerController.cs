using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ScoreManager scoreManager;

    [SerializeField] private bool facingRight = true;
    [SerializeField] private bool jump;
    [SerializeField] private bool grounded;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public float forceJumpLimit = 1700f;
    public float HorizontalJumpFactor = 100f;

    public ParticleSystem forceJumpEffect;

    private Animator animator;
    private Rigidbody2D rb;

    

    private Vector3 lastFrameVelocity;


    private void Awake()
    {
        jump = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lastFrameVelocity = rb.velocity;
        animator.SetBool("Grounded", grounded);
        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(h));

        if (Mathf.Abs(h * rb.velocity.x) < maxSpeed)
            rb.AddForce(h * moveForce * Vector2.right);

        if (Mathf.Abs(h) <= 0.05) rb.velocity = new Vector2(0, rb.velocity.y);


        if ((h > 0 && !facingRight) || (h < 0 && facingRight)) Flip();

        if (jump)
        {
            float totalJumpForce =
                jumpForce + Mathf.Abs(rb.velocity.x) * HorizontalJumpFactor;
            if (totalJumpForce > forceJumpLimit)
                forceJumpEffect.Play();
            rb.AddForce(Vector2.up * totalJumpForce);
            jump = false;
        }
    }


    /// <summary>
    /// Flip Player 
    /// </summary>
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Flip();
            Bounce(col.contacts[0].normal);
        }

        if (col.gameObject.tag == "Platform")
        {
            scoreManager.UpdateScore((int) transform.position.y);
            grounded = true;
        }
        
    }
    
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            grounded = false;
        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        rb.velocity = direction * Mathf.Max(speed, 0);
    }
}