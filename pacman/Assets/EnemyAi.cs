using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{
    //ghosts
    NavMeshAgent agent;
    public Transform agent2;
    public Transform agent3;
    public Transform agent4;

    Transform player;
    public Transform position;
    public LayerMask whatIsGround;

    //Patroling
    Vector3 walkPoint, startPoint;
    public float walkPointRange;
    public float countdown;
    bool walkPointSet, awake, reset = false;
    public bool escape, dead;
    float app, countdownDeadlock, clock;

    int viteAttuali;
    Material scared, mio, white;

    public Text powerUp, vite;

    private void Awake()
    {
        player = GameObject.Find("Chomp").transform;
        agent = this.GetComponent<NavMeshAgent>();

        //inizializzo valori 
        clock = 0.0f;
        app = countdown;
        viteAttuali = 3;
        walkPointSet = false;
        awake = false;
        escape = true;
        dead = false;
        countdownDeadlock = 1;
        startPoint = this.transform.position;
        agent.SetDestination(startPoint);     

        //estrapolo i materiali
        string namepath = agent.name.Split(' ')[0];
        namepath = namepath.Split('_')[0] + namepath.Split('_')[1];
        mio = Resources.Load("Materials/Characters/" + namepath + "_MAT", typeof(Material)) as Material;
        scared = Resources.Load("Materials/Characters/ScaredGhost_MAT", typeof(Material)) as Material;
        white = Resources.Load("Materials/Characters/Ghost_MAT", typeof(Material)) as Material;
    }

    private void Update()
    {
        if (reset)
        {
            if ((transform.position - startPoint).magnitude < 0.5f)
            {
                escape = false;
                dead = false;
                awake = false;
                countdown = app + (8.0f - clock);
                reset = false;
                clock = 0f;
            }
            else
            {
                clock += Time.deltaTime;
            }
        }
        else
        {
            //un fantasma ha catturato il pacman?
            if (int.Parse(vite.text.Split(' ')[3]) != viteAttuali)
            {
                reset = true;
                walkPoint = startPoint;
                walkPointSet = true;
                agent.SetDestination(walkPoint);
                viteAttuali = int.Parse(vite.text.Split(' ')[3]);
            }

            //il fantasma deve rimanere nello spawn o può iniziare la ricerca?
            if (!awake) { StartCoroutine(TimerAwake()); }
            else
            {
                //il pacman ha il potere? 
                if (float.Parse(powerUp.text) != 0.0f)
                {   //se il ghost è stato mangiato non teme più il pacman ma se questo mangia un'altro power, il ghost è di nuovo in pericolo
                    if (float.Parse(powerUp.text) >= 9.9f) escape = true;

                    //il fantasma deve scappare?
                    if (escape) StartCoroutine(goAway());
                    else walk();
                }
                else
                {
                    walk();
                }
            }

            if ((transform.position - player.position).magnitude < 0.8f)
            {
                if (float.Parse(powerUp.text) > 0.0f && escape && !dead)
                {
                    this.GetComponent<Renderer>().material = white;
                    agent.speed = 6f;
                    escape = false;
                    dead = true;
                    walkPoint = startPoint;
                    agent.SetDestination(startPoint);
                }
            }
        }
    }

    private void walk()
    {

        if (!dead)
        {
            Vector3 distanceToPacman = transform.position - player.position;

            if (distanceToPacman.magnitude >= 7.5f)
            {
                Patroling();
            }
            else
            {
                walkPointSet = false;
                ChasePlayer();
            }
        }
        else
        {
            if ((transform.position - walkPoint).magnitude < 0.5f)
            {
                agent.speed = 4f;
                dead = false;
                walkPointSet = true;
                this.GetComponent<Renderer>().material = mio;
                walkPoint = position.position;
                agent.SetDestination(walkPoint);
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

            if (distance2.magnitude < 1.25f || distance3.magnitude < 1.25f || distance4.magnitude < 1.25f)
            {
                StartCoroutine(deadlock());
            }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 2f)
                walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange - 4, walkPointRange + 4);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomChange = 1;

        if ((this.transform.position.x + randomX) > 6f || (this.transform.position.x + randomX) < -7f) { randomX = 0.5f; }
        if ((this.transform.position.z + randomZ) > 10f || (this.transform.position.z + randomZ) < -10f) { randomZ = 0.5f; }

        if (Random.Range(0, 4) % 2 == 0) { randomChange = randomChange * -1; };
        walkPoint = new Vector3(this.transform.position.x + randomX, this.transform.position.y, (this.transform.position.z + randomZ) * randomChange);
        walkPointSet = true;

        if (Physics.Raycast(walkPoint, -this.transform.up, 8f, whatIsGround))
            walkPointSet = true;

        agent.SetDestination(walkPoint);
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    private IEnumerator TimerAwake()
    {
        if (countdown <= 0)
        {
            awake = true;
            walkPointSet = true;

            walkPoint = position.position;
            agent.SetDestination(walkPoint);
        }
        else
        {
            countdown -= Time.deltaTime;
        }
        yield return null;
    }

    private IEnumerator goAway()
    {
        if (float.Parse(powerUp.text) > 0.1f)
        {
            agent.SetDestination(player.position * -1);
            this.GetComponent<Renderer>().material = scared;
            agent.speed = 3.5f;
        }
        else
        {
            this.GetComponent<Renderer>().material = mio;
            walkPointSet = true;
            agent.SetDestination(position.position);
        }
        yield return null;
    }

    private IEnumerator deadlock()
    {
        if (countdownDeadlock >= 0.0f)
        {
            countdownDeadlock -= Time.deltaTime;
        }
        else
        {
            if (walkPoint.x > 0.0f) walkPoint.x *= -1;
            if (walkPoint.z > 0.0f) walkPoint.z *= -1;
            agent.SetDestination(walkPoint);
            countdownDeadlock = 1;
        }
        yield return null;
    }
}