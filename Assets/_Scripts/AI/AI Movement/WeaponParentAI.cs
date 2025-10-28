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
        //depending on where player is, flip both weapon and character sprites
        if (direction.x < 0)
        {
            scale.y = -1;
            if (characterRenderer != null)
                characterRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
            if (characterRenderer != null)
                characterRenderer.flipX = false;
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

    //For ranged weapons
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

    //For melee weapons
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

    //Coroutine to delay ranged attack
    private IEnumerator DelayRangedAttack()
    {
        yield return new WaitForSeconds(rangedDelay);
        attackBlocked = false;
    }

    //Coroutine to delay melee attack
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(meleeDelay);
        attackBlocked = false;
    }

    //Visualize melee attack range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    //For melee weapons to detect colliders in range
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