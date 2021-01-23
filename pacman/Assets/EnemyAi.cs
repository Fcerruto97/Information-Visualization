
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi: MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;
    //private Rigidbody rigidBody;


    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet = true;
    public float walkPointRange;

    void Start()
    {
        //rigidBody = this.GetComponent<Rigidbody>();
        agent = this.GetComponent<NavMeshAgent>();
        //rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;        
    }

    private void Awake()
    {
        player = GameObject.Find("Chomp").transform;
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Vector3 distanceToPacman = this.transform.position - player.transform.position;

        if (distanceToPacman.magnitude > 6f) { Patroling(); Debug.Log("patrol"); }
            else { ChasePlayer(); walkPointSet = false; Debug.Log("Chase"); }

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        Vector3 distanceToWalkPoint = this.transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        if((transform.position.x + randomX)>24f || (transform.position.x + randomX) < -24f) { randomX = 0; }
        if ((transform.position.z + randomZ) > 58f || (transform.position.z + randomZ) < -15f) { randomZ = 0; }


        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
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

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            //this.transform.localPosition = new Vector3(0f + posX, 0.084f, 0.615f + posZ);
        }

    }
}
