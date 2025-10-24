using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private Rigidbody2D bulletRB;
    // When true the bullet can damage enemies (used for reflected bullets)
    public bool canDamageEnemies = false;
    // optional owner reference
    public GameObject owner;

    void Start()
    {   
        // Get Rigidbody2D component
        bulletRB = GetComponent<Rigidbody2D>();

        // If target is set, shoot toward target
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
        // Ignore collisions with enemies if this bullet isn't allowed to damage them
        if (collision.CompareTag("Enemy") && !canDamageEnemies)
            return;
            
        // Try to find a Health component on what we hit
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            // If this bullet was reflected and has an owner, attribute damage to the owner (e.g. player)
            GameObject sender = owner != null ? owner : gameObject;
            health.GetHit(1, sender);
        }

        // Destroy the bullet after collision
        Destroy(this.gameObject);
    }


}
