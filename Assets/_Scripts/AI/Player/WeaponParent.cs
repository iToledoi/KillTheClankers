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
        //depending on where pointer is facing, weapon will flip accordingly
        if(direction.x < 0)
        {
            scale.y = -1;
        }else if(direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) 
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

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

    private IEnumerator DelayRangedAttack()
    {
        yield return new WaitForSeconds(rangedDelay);
        attackBlocked = false;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(meleeDelay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

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
