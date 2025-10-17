using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private Rigidbody2D bulletRB;

    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();

        if (target != null)
        {
            // Calculate direction toward player at the moment of firing
            Vector2 direction = (target.position - transform.position).normalized;
            bulletRB.velocity = direction * speed;

            // Rotate the bullet sprite to face direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Destroy bullet after 2 seconds to prevent clutter
        Destroy(this.gameObject, 4f);
    }

    // Called automatically when bullet collides with something (with collider)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions with other bullets or the shooter if needed
        if (collision.CompareTag("Enemy"))
            return;

        // Try to find a Health component on what we hit
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            health.GetHit(1, gameObject);
        }

        // Destroy the bullet after collision
        Destroy(this.gameObject);
    }


}
