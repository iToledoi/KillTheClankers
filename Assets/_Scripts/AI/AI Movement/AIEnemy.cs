using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    private WeaponParentAI weaponParent;

    [SerializeField]
    private bool ranged = true;
    [SerializeField]
    private float attackRange;
    private bool isDead = false;
    [SerializeField]
    private AudioClip[] deathSounds;

    // Get weapon parent component
    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParentAI>();
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

    }

    // Called to mark the enemy as dead, plays death sound and adds to score
    private void killEnemy()
    {
        isDead = true;
        SoundFXManager.instance.PlayRandomSound(deathSounds, transform, 1f);
        //Update UI score
        ScoreManager.instance.AddScore();
    }
}