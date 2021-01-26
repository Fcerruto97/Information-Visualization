using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{
    //ghosts
    public NavMeshAgent agent;
    public Transform agent2;
    public Transform agent3;
    public Transform agent4;

    public Transform player;
    public Transform position;
    public LayerMask whatIsGround;

    //Patroling
    Vector3 walkPoint;
    bool walkPointSet = false;
    public float walkPointRange;
    bool awake = false;
    public float countdown;
    float countdownPowerUp = 10;
    int count = 0;

    Material scared, mio;

    public Text powerUp;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Awake()
    {
        player = GameObject.Find("Chomp").transform;
        agent = this.GetComponent<NavMeshAgent>();
        string namepath = agent.name.Split(' ')[0];
        namepath = namepath.Split('_')[0]+namepath.Split('_')[1];
        Debug.Log(namepath);
        mio = Resources.Load("Materials/Characters/" + namepath + "_MAT", typeof(Material)) as Material;
        //white = Resources.Load("Materials/Characters/Ghost_MAT", typeof(Material)) as Material;
        scared = Resources.Load("Materials/Characters/ScaredGhost_MAT", typeof(Material)) as Material;
    }

    private void Update()
    {
        if (powerUp.text.Equals("true"))
        {            
            StartCoroutine(goAway());
        }
        else
        {
            if (!awake) { StartCoroutine(TimerAwake()); }
            else
            {
                Vector3 distanceToPacman = this.transform.position - player.position;

                if (distanceToPacman.magnitude > 7f)
                {
                    Patroling();
                    //Debug.Log("Patrol "+ agent.name);
                }
                else
                {
                    ChasePlayer();
                    walkPointSet = false;
                    //Debug.Log("Chase " + agent.name); 
                }
            }
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        else
        {
            Vector3 distance2 = this.transform.position - agent2.position;
            Vector3 distance3 = this.transform.position - agent3.position;
            Vector3 distance4 = this.transform.position - agent4.position;

            if (distance2.magnitude < 0.5f || distance3.magnitude < 0.5f || distance4.magnitude < 0.5f)
            {
                SearchWalkPoint();
            }

            Vector3 distanceToWalkPoint = this.transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1.5f)
                walkPointSet = false;
        }
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
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, (transform.position.z + randomZ) * randomChange);
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
        if (countdown <= 0)
        {
            awake = true;
            agent.SetDestination(position.position);
        }
        else
        {
            countdown -= Time.deltaTime;
        }
        yield return null;
    }

    public IEnumerator goAway()
    {
        if (countdownPowerUp >=0)
        {
            agent.SetDestination(player.position * -1);
            this.GetComponent<Renderer>().material = scared;
            countdownPowerUp -= Time.deltaTime;
        }
        else
        {
            this.GetComponent<Renderer>().material = mio;
            countdownPowerUp = 10;
            walkPointSet = true;
            agent.SetDestination(position.position);
        }
        yield return null;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "ghost")
        {
            SearchWalkPoint();
        }
        
    }
}
