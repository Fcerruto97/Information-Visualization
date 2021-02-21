using System.Collections;
using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class PacmanAgent : Agent
{
    public Text vite, powerUp;
    public float speed, dist;
    private Rigidbody rigidBody;
    bool power = false;
    private int life, countEat, countSfereEat;
    float countdownPowerUp, countdownReset;
    bool reset = false;
    Vector3 startPosition;
    public GameObject gruppo;

    // Start is called before the first frame update
    public override void Initialize()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        countdownPowerUp = 0f;
        countdownReset = 7.0f;
        life = 3;
        power = false;
        countEat = 0;
        countSfereEat = 240;
        vite.text = "Vite rimaste = " + life;        
    }

    public override void OnEpisodeBegin(){
        Debug.Log("New Game");    
        Reset();
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();

        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
            discreteActionsOut[2] = 3;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
            discreteActionsOut[2] = 4;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);        
    }

    //da modificare
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (reset) { StartCoroutine(ResetLevel()); }
        else
        {
            if (power) { StartCoroutine(PowerEat()); }

            MoveAgent(actionBuffers.DiscreteActions);

            if (transform.position.x > 9.4f + dist && transform.position.z > 0.5)
            {
                transform.position = new Vector3(-8.5f + dist, 0f, 0.5f);
            }

            if (transform.position.x < -8.9f + dist && transform.position.z > 0.5)
            {
                transform.position = new Vector3(9.0f + dist, 0f, 0.5f);
            }            
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {

        AddReward(-0.000001f);

        var dirToGo = Vector3.zero;        
        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotatAxis = act[2];
        int rotateDir = 1;
       
        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * speed;
                break;
            case 2:
                dirToGo = transform.forward * -speed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * speed;
                break;
            case 2:
                dirToGo = transform.right * -speed;
                break;
        }

        switch (rotatAxis)
        {
            case 1:
                rotateDir = 0;
                break;
            case 2:
                rotateDir = 180;
                break;
            case 3:
                rotateDir = -90;
                break;
            case 4:
                rotateDir = 90;
                break;
        }
        
        transform.rotation = Quaternion.AngleAxis(rotateDir, Vector3.up);
        rigidBody.AddForce(dirToGo, ForceMode.VelocityChange);
    }

    private void FixedUptade()
    {
        //qualcosa da aggiungere sicuro video minuto 8
        //fare qualcosa prima di chiamare il request decision
        RequestDecision();
    }

    private void Reset()
    {
        //verificare settaggio parametri iniziale
        transform.position = startPosition;
        countdownPowerUp = 0f;
        countdownReset = 7.0f;
        life = 3;
        reset = true;
        power = false;
        countEat = 0;
        countSfereEat = 240;
        rigidBody = GetComponent<Rigidbody>();
        vite.text = "Vite rimaste = " + life;
    
        Component[] insiemeSfere = gruppo.GetComponentsInChildren<Transform>(true);
        foreach (var sfera in insiemeSfere) {
            sfera.gameObject.SetActive(true);
        }
    }

    // aggiustare tutti i reward
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bonus")
        {
            AddReward(0.015f);
            //Debug.Log(GetCumulativeReward());
            col.gameObject.SetActive(false);
            if (countSfereEat == 1)
            {
                AddReward(5f);
                //Debug.Log("Vittoria");
                Debug.Log("VITTORIA EndEpisode = " + /*GetCumulativeReward());+*/ " " + this.name);
                EndEpisode();
            }
            else {
                countSfereEat--;
            }
        }

        if (col.gameObject.tag == "cerry")
        {
            AddReward(0.01f);
            //Debug.Log(GetCumulativeReward());
            col.gameObject.SetActive(false);
        }

        if (col.gameObject.tag == "ghost")
        {
            Collision(col.GetComponent<EnemyAi>());
        }

        if (col.gameObject.tag == "power")
        {
            AddReward(0.0155f);
            //Debug.Log(GetCumulativeReward());            
            col.gameObject.SetActive(false);
            power = true;
            countEat = 0;
            RequestDecision();
            countdownPowerUp += 10;
        }
    }

    private void gameOver()
    {
        life--;
        if (life > 0)
        {
            AddReward(-2f);
            //Debug.Log(GetCumulativeReward());
            vite.text = "Vite rimaste = " + life;
            transform.position = startPosition;
            reset = true;
        }
        else
        {
            AddReward(-2.5f);
            //Debug.Log("Perso");
            Debug.Log("SCONFITTA EndEpisode = "/*+GetCumulativeReward() */+ " " + this.name);
            EndEpisode();
        }
    }

    private void Collision(EnemyAi ghost)
    {
        if (!ghost.GetComponent<EnemyAi>().dead)
        {
            if (power && ghost.GetComponent<EnemyAi>().escape)
            {
                AddReward(0.3f);
                //Debug.Log(GetCumulativeReward());
                countEat++;
            }
            else
            {
                gameOver();
            }
        }
    }

    public IEnumerator PowerEat()
    {
        if (countdownPowerUp >= 0)
        {
            countdownPowerUp -= Time.deltaTime;
            powerUp.text = "" + countdownPowerUp;
        }
        else
        {
            power = false;
            powerUp.text = "" + 0;
        }
        yield return null;
    }

    public IEnumerator ResetLevel()
    {
        if (countdownReset >= 0)
        {
            countdownReset -= Time.deltaTime;
        }
        else
        {
            reset = false;
            countdownReset = 7.0f;
        }
        yield return null;
    }
}
