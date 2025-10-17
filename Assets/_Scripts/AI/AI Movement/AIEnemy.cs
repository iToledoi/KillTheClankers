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

    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParentAI>();
    }

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
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (player != null)
        {
            // Continuously update the destination
            agent.SetDestination(player.position);

            float distanceFromPlayer = Vector2.Distance(player.position,transform.position);
            if (distanceFromPlayer < attackRange)
            {
                PerformAttack();
            }
            
        }

    }
}