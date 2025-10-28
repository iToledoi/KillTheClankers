using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;

    public Vector2 PointerPosition { get; set; }

    public Animator animator;

    public float rangedDelay = 2f;
    public float meleeDelay = 0.3f;
    private bool attackBlocked = false;

    [SerializeField]
    private AudioClip[] swordSounds;

    public Transform circleOrigin;
    public float radius;

    //Property
    public bool IsAttacking {  get; set; }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    private void Update()
    {   
        //Don't want weapon to change animation position when attacking
        if(IsAttacking)
        {
            return;
        }

        //Gets position of mouse pointer for direction weapon should be facing
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        //depending on where pointer is facing, flip both weapon and character sprites
        if(direction.x < 0)
        {
            scale.y = -1;
            if (characterRenderer != null)
                characterRenderer.flipX = true;
                
        }else if(direction.x > 0)
        {
            scale.y = 1;
            if (characterRenderer != null)
                characterRenderer.flipX = false;
        }
        transform.localScale = scale;

        //Adjust weapon sorting order based on angle
        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) 
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }
    // Ranged attack: shoot projectile towards pointer
    public void RangedAttack()
    {
        if (attackBlocked)
        {
            return;
        }
        //animator.SetTrigger("RangedAttack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayRangedAttack());
    }

    // Melee attack: swing sword in front of player
    public void MeleeAttack()
    {
        if (attackBlocked)
        {
            return;
        }
        //play melee attack sounds here
        SoundFXManager.instance.PlayRandomSound(swordSounds, transform, 0.5f);
        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    // Delay between ranged attacks
    private IEnumerator DelayRangedAttack()
    {
        yield return new WaitForSeconds(rangedDelay);
        attackBlocked = false;
    }

    // Delay between melee attacks
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(meleeDelay);
        attackBlocked = false;
    }

    // Visualize melee attack radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    // Detect colliders within melee attack radius and apply damage
    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            Debug.Log(collider.name);

            Health health;
            if(health = collider.GetComponent<Health>())
            {
                health.GetHit(1, transform.parent.gameObject);
            }
        }
    }
}
