using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const int defaultJumpValue = 2;
    public const float runningSpeed = 9;
    public const float defaultSpeed = 4;
    public const float rotation = 0;

    public Animator animator;

    public float horizontalInput;
    public float speed = defaultSpeed;
    public float jumpForce;
    public float jumpCooldown = 0.3f; 

    public Rigidbody2D rig;
    public SpriteRenderer renderer;

    int jumpsLeft = defaultJumpValue;
    bool isGrounded = true;
    float lastJumpTime = 0f;

    void Start()
    {
        rig.interpolation = RigidbodyInterpolation2D.Interpolate;
        animator = GetComponent<Animator>();

        rig.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        Move();
        Jump();
        Flip();
        Run();
    }

    void Move()
    {
      
        horizontalInput = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(horizontalInput * speed, rig.velocity.y);
        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            animator.SetBool("Walking", true);
            
        }
        else
        {
            animator.SetBool("Walking", false);
            
        }
    }

    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.J) && jumpsLeft > 0 && Time.time > lastJumpTime + jumpCooldown)
        {
            if (isGrounded || jumpsLeft > 0)
            {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                jumpsLeft--;
                lastJumpTime = Time.time;
                animator.SetBool("Jumping", true);
                isGrounded = false; 
            }
        }
        else
        {
            animator.SetBool("Jumping", false);
        }
    }

    void Flip()
    {
        
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Run()
    {
        
        if (Input.GetKey(KeyCode.K))
        {
            animator.SetBool("Running", true);
            speed = runningSpeed;
        }
        else
        {
            animator.SetBool("Running", false);
            speed = defaultSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plataform"))
        {
            isGrounded = true;
            jumpsLeft = defaultJumpValue; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plataform"))
        {
            isGrounded = false;
        }
    }
}
