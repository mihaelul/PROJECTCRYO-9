using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // States
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;


    private void Awake()
    {
        player = GameObject.Find("PlayerModel").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
{
    playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    Debug.Log($"Sight: {playerInSightRange}, Attack: {playerInAttackRange}, WalkPointSet: {walkPointSet}");

    if (!playerInSightRange && !playerInAttackRange)
    {
        Patroling();
    }
    else if (playerInSightRange)
    {
        ChasePlayer();
    }
    else if (!playerInSightRange)
    {
        walkPointSet = false;
        Patroling();
    }
}

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
                Debug.Log("WalkPoint reached");
            }
        }
    }

         private void SearchWalkPoint()
    {
       
        Vector3 randomDirection = new Vector3(
            Random.Range(-walkPointRange, walkPointRange),
            0f,
            Random.Range(-walkPointRange, walkPointRange)
        );
        Vector3 potentialPoint = transform.position + randomDirection;

        RaycastHit groundHit;
        if (Physics.Raycast(potentialPoint + Vector3.up * 10f, Vector3.down, out groundHit, 20f, whatIsGround))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(groundHit.point, out navHit, 2.0f, NavMesh.AllAreas))
            {
                walkPoint = navHit.position;
                walkPointSet = true;
                Debug.Log("WalkPoint set at: " + walkPoint);
            }
            else
            {
                Debug.Log("No valid NavMesh point found");
            }
        }
        else
        {
            Debug.Log("No ground found for walk point");
        }
    }
}