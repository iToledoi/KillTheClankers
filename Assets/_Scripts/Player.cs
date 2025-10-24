using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private Vector2 pointerInput;

    [SerializeField]
    private InputActionReference meleeAttack, pointerPosition, special;

    [Header("Special Move")]
    public float specialRange = 5f;
    [Tooltip("Half-angle of cone in degrees (e.g. 45 means 90 degree cone)")]
    public float specialHalfAngle = 45f;
    public float specialForce = 12f;
    public float specialCooldown = 2f;
    private bool specialReady = true;
    private bool isDead = false;

    private WeaponParent weaponParent;

    // Subscribe to input events
    private void OnEnable()
    {
        meleeAttack.action.performed += PerformAttack;
        if (special != null)
            special.action.performed += PerformSpecial;

    }

    // Unsubscribe from input events
    private void OnDisable()
    {
        meleeAttack.action.performed -= PerformAttack;
        if (special != null)
            special.action.performed -= PerformSpecial;

    }

    // Perform melee attack via weapon parent
    private void PerformAttack(InputAction.CallbackContext obj)
    {
        weaponParent.MeleeAttack();
    }

    //Get weapon parent component
    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    // Update is called once per frame
    void Update()
    {
        pointerInput = GetPointerInput();
        weaponParent.PointerPosition = pointerInput;

    }

    // Special move: knockback enemies and reflect projectiles in a cone in front of player
    private void PerformSpecial(InputAction.CallbackContext obj)
    {
        if (!specialReady) return;
        StartCoroutine(DoSpecial());
    }

    // Coroutine to handle special move logic and cooldown
    private IEnumerator DoSpecial()
    {
        specialReady = false;

        Vector2 origin = transform.position;
        Vector2 pointerWorld = GetPointerInput();
        Vector2 forward = (pointerWorld - origin).normalized;

        // Find potential targets in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, specialRange);
        foreach (var col in hits)
        {
            if (col == null) continue;
            // skip self
            if (col.transform == this.transform) continue;

            // Only affect enemies (by tag) or objects with KnockbackFeedback/rigidbody
            bool isEnemy = col.CompareTag("Enemy");
            var hasKb = col.GetComponent<KnockbackFeedback>() != null;
            var rb = col.attachedRigidbody ?? col.GetComponent<Rigidbody2D>();
            var isBullet = col.GetComponent<Shooting>() != null;
            if (!isEnemy && !hasKb && rb == null && !isBullet) continue;

            Vector2 toTarget = (col.transform.position - transform.position);
            float dist = toTarget.magnitude;
            if (dist <= 0.001f) continue;
            Vector2 dir = toTarget.normalized;

            float angle = Vector2.Angle(forward, dir);
            if (angle <= specialHalfAngle)
            {
                // If it's a bullet, reflect it so it can damage enemies
                if (isBullet)
                {
                    var shot = col.GetComponent<Shooting>();
                    if (shot != null)
                    {
                        // reverse velocity away from player
                        if (rb != null)
                        {
                            Vector2 away = (col.transform.position - transform.position).normalized;
                            rb.velocity = away * Mathf.Max(rb.velocity.magnitude, 1f);
                        }
                        shot.canDamageEnemies = true;
                        shot.owner = this.gameObject;
                        // Ensure this bullet will collide with enemies now that it's reflected.
                        int enemyLayer = LayerMask.NameToLayer("Enemy");
                        int reflectedLayer = LayerMask.NameToLayer("ReflectedProjectile");
                        if (reflectedLayer != -1)
                        {
                            col.gameObject.layer = reflectedLayer;
                        }
                        else
                        {
                            int defaultLayer = LayerMask.NameToLayer("Default");
                            if (defaultLayer != -1)
                                col.gameObject.layer = defaultLayer;
                        }

                        if (enemyLayer != -1)
                        {
                            // Ensure layer collision between this bullet's layer and Enemy layer is enabled
                            Physics2D.IgnoreLayerCollision(col.gameObject.layer, enemyLayer, false);
                        }
                    }
                    continue; // bullets are handled, skip knockback
                }

                // Apply knockback to enemies or objects
                var kb = col.GetComponent<KnockbackFeedback>();
                if (kb != null)
                {
                    // call with this gameobject as sender so feedback computes direction
                    kb.PlayFeedback(gameObject);
                }
                else if (rb != null)
                {
                    // push away from player
                    Vector2 force = dir * specialForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }

        // simple visual or sound hook could be triggered here

        // cooldown
        yield return new WaitForSeconds(specialCooldown);
        specialReady = true;
    }

    //Function gets mouse input from user to determine where character faces
    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

}
