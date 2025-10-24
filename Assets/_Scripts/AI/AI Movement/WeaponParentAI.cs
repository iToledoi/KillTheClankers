using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParentAI : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;

    public Transform player;

    public Animator animator;

    public float rangedDelay = 2f;
    public GameObject bullet;
    public GameObject shooting;
    [SerializeField]
    private AudioClip[] gunSounds;

    public float meleeDelay = 0.3f;
    private bool attackBlocked = false;

    public Transform circleOrigin;
    public float radius;

    //Property to check if the weapon is currently attacking
    public bool IsAttacking { get; set; }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    private void Update()
    {

        if (player == null)
        {
            return;
        }
        //Don't want weapon to change animation position when attacking
        if (IsAttacking)
        {
            return;
        }

        //Gets position of player for direction weapon should be facing
        Vector2 direction = player.position - transform.position;
        //Rotate weapon toward the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector2 scale = transform.localScale;
        //depending on where pointer is facing, weapon will flip accordingly
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
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
        animator.SetTrigger("RangedAttack");
        IsAttacking = true;
        attackBlocked = true;
        GameObject newBullet = Instantiate(bullet, shooting.transform.position, shooting.transform.rotation);

        //play ranged attack sounds here
        SoundFXManager.instance.PlayRandomSound(gunSounds, transform, 1f);
            
        Shooting bulletScript = newBullet.GetComponent<Shooting>();
        if (bulletScript != null)
        {
            bulletScript.target = player;
        }
        StartCoroutine(DelayRangedAttack());
    }
    public void MeleeAttack()
    {
        if (attackBlocked)
        {
            return;
        }
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

    //For melee weapons
    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            //Debug.Log(collider.name);

            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(1, transform.parent.gameObject);
            }
        }
    }

}