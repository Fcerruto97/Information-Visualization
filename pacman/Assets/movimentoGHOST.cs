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

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    public override void OnEpisodeBegin()
    {
        this.rigidBody.angularVelocity = Vector3.zero;
        this.rigidBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0f + posX, 0.084f, 0.615f + posZ);
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 move = Vector3.zero;
        move.x = vectorAction[0];

        if (vectorAction[1] == 2)
        {
            move.z = 1;
        } else
        {
            move.z = -vectorAction[1];
        }

        rigidBody.AddForce(move * speed);

        if (transform.position.x > 9.4f)
        {
            transform.position = new Vector3(-8.5f, 0.084f, 0.69f);
        }

        if (transform.position.x < -8.9f)
        {
            transform.position = new Vector3(9.0f, 0.084f, 0.69f);
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        if (distanceToTarget < 1f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        if (sfere.text.Equals("0"))
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

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
