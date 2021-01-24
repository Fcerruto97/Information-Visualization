
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi: MonoBehaviour
{
    public NavMeshAgent agent;
    public NavMeshAgent agent2;
    public NavMeshAgent agent3;
    public NavMeshAgent agent4;

    public Transform player;
    public Transform position;
    public LayerMask whatIsGround;
    
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet = true;
    public float walkPointRange;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();        
    }

    private void Awake()
    {
        StartCoroutine(TimerAwake());
        player = GameObject.Find("Chomp").transform;
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(position.position);
        walkPointSet = false;
    }

    private void Update()
    {
        Vector3 distanceToPacman = this.transform.position - player.transform.position;

        if (distanceToPacman.magnitude > 6f) { Patroling(); Debug.Log("patrol "+ agent.name); }
            else { ChasePlayer(); walkPointSet = false; Debug.Log("Chase" + agent.name); }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        Vector3 distance2 = this.transform.position - agent2.transform.position;
        Vector3 distance3 = this.transform.position - agent3.transform.position;
        Vector3 distance4 = this.transform.position - agent4.transform.position;

        if (distance2.magnitude < 1f || distance3.magnitude < 1f || distance4.magnitude < 1f)
        {
            SearchWalkPoint();
        }

        Vector3 distanceToWalkPoint = this.transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1.5f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomChange = 1;

        if ((transform.position.x + randomX) > 6f || (transform.position.x + randomX) < -7f) { randomX = 0.5f; }
        if ((transform.position.z + randomZ) > 10f || (transform.position.z + randomZ) < -10f) { randomZ = 0.5f; }

        if (Random.Range(0, 4) % 2 == 0) { randomChange = randomChange * -1; };
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, (transform.position.z + randomZ)* randomChange);
        walkPointSet = true;

        if (Physics.Raycast(walkPoint, -transform.up, 8f, whatIsGround))
            walkPointSet = true;

        agent.SetDestination(walkPoint);
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private IEnumerator TimerAwake()
    {
    }
}
