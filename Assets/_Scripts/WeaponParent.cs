using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;

    public Vector2 PointerPosition { get; set; }

    public Animator animator;

    public float meleeDelay = 0.3f;
    private bool attackBlocked = false;

    private void Update()
    {   
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

    public void MeleeAttack()
    {
        if (attackBlocked)
        {
            return;
        }
        animator.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(meleeDelay);
        attackBlocked = false;
    }
}
