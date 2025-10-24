using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private float speedX;
    private float speedY;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");

        Vector2 moveInput = new Vector2(speedX, speedY).normalized;
        if (moveInput != Vector2.zero)
            lastMoveDir = moveInput;

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            Debug.Log("Dashing");
            // Play dash sound
            SoundFXManager.instance.PlaySound(dashSound, transform, 2f);
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        // skip normal movement during dash
        if (isDashing)
            return; 

        rb.velocity = new Vector2(speedX * moveSpeed, speedY * moveSpeed);
    }

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