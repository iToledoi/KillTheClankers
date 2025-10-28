using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private float speedX;
    private float speedY;

    [SerializeField]
    private Animator animator;
    
    [SerializeField]
    [Tooltip("Minimum movement speed to consider the player 'moving' (in units/sec). Helps avoid animation flicker.")]
    private float movementThreshold = 0.1f;

    //Movement vars
    public float moveSpeed = 10f;
    private Vector2 lastMoveDir; //stores last movement direction

    //Dashing vars
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 20f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1.5f;

    [SerializeField]
    private AudioClip dashSound;

    // Initialize references
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // try to auto-assign animator if not set in inspector
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Handle movement input
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");
        // Normalize diagonal movement
        Vector2 moveInput = new Vector2(speedX, speedY).normalized;
        if (moveInput != Vector2.zero)
            lastMoveDir = moveInput;

        // Handle dash input
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            Debug.Log("Dashing");
            // Play dash sound
            SoundFXManager.instance.PlaySound(dashSound, transform, 2f);
            StartCoroutine(Dash());
        }
    }

    // Apply movement physics
    void FixedUpdate()
    {
        // skip normal movement during dash
        if (isDashing)
        {
            // keep playing moving animation while dashing
            if (animator != null)
                animator.SetBool("isMoving", true);
            return;
        }

        rb.velocity = new Vector2(speedX * moveSpeed, speedY * moveSpeed);

        // update animator based on actual Rigidbody2D velocity to reflect real movement (including physics)
        if (animator != null)
        {
            bool moving = rb.velocity.sqrMagnitude > (movementThreshold * movementThreshold);
            animator.SetBool("isMoving", moving);
        }
    }
    
    // Handle dashing coroutine
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = lastMoveDir * dashingPower;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}