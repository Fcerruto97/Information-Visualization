using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class movimentoGHOST : Agent
{
    public float posX, posZ;
    public float speed;
    private Rigidbody rigidBody;
    public Transform target;
   
    public Text sfere;


    //public Transform Destination;
    //private NavMeshAgent Agent;

    /*void Start()
    {
        Agent = this.GetComponent<NavMeshAgent>();
        if (Agent == null)
        {
            Debug.LogError("Null agent");

        }
        else
        {
            SetDestination();
        }
        //rigidBody = GetComponent<Rigidbody>();
        //rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }*/

   /* void Update()
    {
        Agent.SetDestination(Destination.position);
    }*/

   /* void SetDestination()
    {
        if (Destination != null)
        {
            Vector3 targetVector = Destination.transform.position();
            Agent.SetDestination(targetVector);
        }
    }*/

    // Update is called once per frame
    /*void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
        }

        Vector3 move = new Vector3(h, 0.0f, v);

        rigidBody.AddForce(move * speed);

        if (transform.position.x > 9.4f)
        {
            transform.position = new Vector3(-8.5f, 0.084f, 0.69f);
        }

        if (transform.position.x < -8.9f)
        {
            Debug.Log("(" + transform.position.x + "," + transform.position.y + "," + transform.position.z + ")");
            transform.position = new Vector3(9.0f, 0.084f, 0.69f);
        }
    }*/
}
