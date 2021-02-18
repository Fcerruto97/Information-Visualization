using System.Collections;
using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PacmanAgent : Agent
{
    public Text vite, powerUp;
    public float speed;
    private Rigidbody rigidBody;
    bool power = false;
    private static int life, countEat, countSfereEat;
    private float distance1, distance2, distance3, distance4;
    public Transform ghost1, ghost2, ghost3, ghost4;
    float countdownPowerUp, countdownReset;
    bool reset = false;
    int c = 0;

    // Start is called before the first frame update
    public override void Initialize()
    {
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
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[2] = 2;
        }
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {        
        /*sensor.AddObservation(distance1);
        sensor.AddObservation(distance2);
        sensor.AddObservation(distance3);
        sensor.AddObservation(distance4);*/

        /*sensor.AddObservation(ghost1.localPosition);
        sensor.AddObservation(ghost2.localPosition);
        sensor.AddObservation(ghost3.localPosition);
        sensor.AddObservation(ghost4.localPosition);*/
                
        sensor.AddObservation(transform.InverseTransformDirection(rigidBody.velocity));
    }

    //da modificare
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (reset) { StartCoroutine(ResetLevel()); }
        else
        {
            if (power) { StartCoroutine(PowerEat()); }

            MoveAgent(actionBuffers.DiscreteActions);

            if (transform.position.x > 9.4f)
            {
                transform.position = new Vector3(-8.5f, 0f, 0.69f);
            }

            if (transform.position.x < -8.9f)
            {
                transform.position = new Vector3(9.0f, 0f, 0.69f);
            }

            /*distance1 = Vector3.Distance(transform.position, ghost1.position);
            distance2 = Vector3.Distance(transform.position, ghost2.position);
            distance3 = Vector3.Distance(transform.position, ghost3.position);
            distance4 = Vector3.Distance(transform.position, ghost4.position);

            Debug.Log("distance= " + distance1 + " " + distance2 + " " + distance3 + " " + distance4);*/

            /*if (distance1 < 1f)
            {
                Collision(ghost1.GetComponent<EnemyAi>());
            }
            if (distance2 < 1f)
            {
                Collision(ghost2.GetComponent<EnemyAi>());
            }
            if (distance3 < 1f)
            {
                Collision(ghost3.GetComponent<EnemyAi>());
            }
            if (distance4 < 1f)
            {
                Collision(ghost4.GetComponent<EnemyAi>());
            }*/
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        AddReward(-0.0001f);
        
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;
        
        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

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

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        rigidBody.AddForce(dirToGo * speed, ForceMode.VelocityChange);
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
        transform.position = new Vector3(0f, 0f, -6.5f);
        countdownPowerUp = 0f;
        countdownReset = 7.0f;
        life = 3;
        power = false;
        countEat = 0;
        countSfereEat = 240;
        rigidBody = GetComponent<Rigidbody>();
        vite.text = "Vite rimaste = " + life;
        if (c == 0) { c++; }
        else {
            SceneManager.LoadScene("Livello");
            Debug.Log("Nuova scena cumulative rewards = "+ GetCumulativeReward());            
        };
    }

    // aggiustare tutti i reward
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bonus")
        {
            AddReward(0.03f);
            //Debug.Log(GetCumulativeReward());
            Destroy(col.gameObject);
            if (countSfereEat == 1)
            {
                AddReward(1f);
                Debug.Log("Vittoria = "+GetCumulativeReward());
                EndEpisode();
            }
            else {
                countSfereEat--;
            }
        }

        if (col.gameObject.tag == "cerry")
        {
            AddReward(0.2f);
            //Debug.Log(GetCumulativeReward());
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "ghost")
        {
            Collision(col.GetComponent<EnemyAi>());
            Debug.Log("Beccato"); 
        }

        if (col.gameObject.tag == "power")
        {
            AddReward(0.1f);
            Debug.Log(GetCumulativeReward());            
            Destroy(col.gameObject);
            power = true;
            countEat = 0;
            countdownPowerUp += 10;
        }
    }

    private void gameOver()
    {
        life--;
        if (life > 0)
        {
            AddReward(-0.5f);
            //Debug.Log(GetCumulativeReward());
            vite.text = "Vite rimaste = " + life;
            transform.position = new Vector3(0f, 0f, -6.5f);
            reset = true;
        }
        else
        {
            AddReward(-1f);
            Debug.Log("Perso = "+GetCumulativeReward());
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
            this.transform.position = new Vector3(0f, 0f, -6.5f);
        }
        yield return null;
    }
}
