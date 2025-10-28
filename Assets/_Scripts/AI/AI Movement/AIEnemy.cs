using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    private WeaponParentAI weaponParent;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool ranged = true;
    [SerializeField]
    private float attackRange;
    private bool isDead = false;

    //Audio clips
    [SerializeField]
    private AudioClip[] deathSounds;
    [SerializeField]
    private AudioClip[] walkingSounds;
    [SerializeField, Range(0.1f, 2f)]
    private float walkingSoundDelay = 0.4f;
    private bool canPlayWalkSound = true;

    // Get weapon parent component
    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParentAI>();
        // try to auto-assign animator if not set in inspector
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Perform attack based on whether the enemy is ranged or melee
    private void PerformAttack()
    {
        if (ranged == true)
        {
            weaponParent.RangedAttack();
        }
        else
        {
            weaponParent.MeleeAttack();
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Try to find player automatically if not set in inspector
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // propagate to weapon parent if present
        if (weaponParent != null && weaponParent.player == null)
        {
            weaponParent.player = player;
        }
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        // Do nothing if dead (stops dead enemies from moving or attacking)
        if (isDead)
        {
            if (animator != null)
                animator.SetBool("isMoving", false);
            return;
        }

        // keep weapon parent's player reference in sync and try to find player if lost
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                if (weaponParent != null)
                    weaponParent.player = player;
            }
        }

        // If player exists, move towards them
        if (player != null)
        {
            // Continuously update the destination
            agent.SetDestination(player.position);

            float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            if (distanceFromPlayer < attackRange)
            {
                PerformAttack();
            }

        }

        // Update animator movement parameter based on agent velocity
        if (animator != null && agent != null)
        {
            float speedSq = agent.velocity.sqrMagnitude;
            bool isMoving = speedSq > 0.01f;
            animator.SetBool("isMoving", isMoving);

            // Play walking sounds only when moving AND allowed by delay
            if (isMoving && canPlayWalkSound && walkingSounds != null && walkingSounds.Length > 0)
            {
                canPlayWalkSound = false;
                SoundFXManager.instance.PlayRandomSound(walkingSounds, transform, 0.6f);
                StartCoroutine(WalkingSoundCooldown());
            }
        }
    }

    private IEnumerator WalkingSoundCooldown()
    {
        yield return new WaitForSeconds(walkingSoundDelay);
        canPlayWalkSound = true;
    }

    // Called to mark the enemy as dead, plays death sound and adds to score
    public void killEnemy()
    {
        isDead = true;
        SoundFXManager.instance.PlayRandomSound(deathSounds, transform, 1f);
        //Update UI score
        ScoreManager.instance.AddScore();
    }
}