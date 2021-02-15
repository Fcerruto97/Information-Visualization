using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class PacmanAgent : Agent
{
    public Text vite, powerUp;
    public float speed;
    private Rigidbody rigidBody;
    private static int count;
    bool power = false;
    private static int life, countEat, countSfereEat;
    private Vector3 distance1, distance2, distance3, distance4;
    public Transform ghost1, ghost2, ghost3, ghost4;
    float countdownPowerUp, countdownReset;

    bool reset = false;

    private Vector3 startingposition = new Vector3(-1f, 0f, 0f), move;
    
    // Start is called before the first frame update
    public override void Initialize()
    {
        rigidBody = GetComponent<Rigidbody>();

        //verificare posizione inziale
        transform.position = startingposition;
        
        this.rigidBody.angularVelocity = Vector3.zero;
        this.rigidBody.velocity = Vector3.zero;
        /*countdownPowerUp = 0f;
        countdownReset = 7.0f;
        count = 0;
        life = 3;
        power = false;
        countEat = 0;
        countSfereEat = 240;
        rigidBody = GetComponent<Rigidbody>();
        //punteggio.text = "Punteggio  = " + count;
        vite.text = "Vite rimaste = " + life;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;*/

    }

    public override void OnEpisodeBegin()
    {
       Reset(); //da aggiustare
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        /*if (reset) { StartCoroutine(ResetLevel()); }
        else
        {*/
            //if (power) { StartCoroutine(PowerEat()); }            

            if (Input.GetKey(KeyCode.UpArrow))
            {
                actionsOut[0] = 1;               
            }
            /*if (Input.GetKey(KeyCode.DownArrow))
            {
                actionsOut[0] = 2;                
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                actionsOut[0] = 3;                
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                actionsOut[0] = 4;               
            }       */     
       // }
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(target.localPosition);
        //sensor.AddObservation(this.transform.localPosition);

       // sensor.AddObservation(this.transform.position);  
    }

    //da modificare
    public override void OnActionReceived(float[] vectorAction)
    {
        if(Mathf.FloorToInt(vectorAction[0])==1)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            move = Vector3.up;
            rigidBody.AddForce(move * speed);
        }

       /* if (Mathf.FloorToInt(vectorAction[0]) == 2)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            move = Vector3.down;
            rigidBody.AddForce(move * speed);
        }

        if (Mathf.FloorToInt(vectorAction[0]) == 3)
        {
            transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
            move = Vector3.right;
            rigidBody.AddForce(move * speed);
        }

        if (Mathf.FloorToInt(vectorAction[0]) == 4)
        {
            transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
            move = Vector3.left;
            rigidBody.AddForce(move * speed);
        }      */  

        //rigidBody.AddForce(move * speed);

        /*if (transform.position.x > 9.4f)
        {
            transform.position = new Vector3(-8.5f, 0.084f, 0.69f);
        }

        if (transform.position.x < -8.9f)
        {
            transform.position = new Vector3(9.0f, 0.084f, 0.69f);
        }

        distance1 = this.transform.position - ghost1.position;
        distance2 = this.transform.position - ghost2.position;
        distance3 = this.transform.position - ghost3.position;
        distance4 = this.transform.position - ghost4.position;

        if (distance1.magnitude < 0.8f)
        {
            Collision(ghost1.GetComponent<EnemyAi>());
        }
        if (distance2.magnitude < 0.8f)
        {
            Collision(ghost2.GetComponent<EnemyAi>());
        }
        if (distance3.magnitude < 0.8f)
        {
            Collision(ghost3.GetComponent<EnemyAi>());
        }
        if (distance4.magnitude < 0.8f)
        {
            Collision(ghost4.GetComponent<EnemyAi>());
        }*/
    }

    private void FixedUptade()
    {
        //qualcosa da aggiungere sicuro video minuto 8
        //fare qualcosa prima di chiamare il request decision
        //RequestDecision();
    }
        
    public void Reset()
    {
        //verificare settaggio paremtri iniziale
        this.transform.localPosition = new Vector3(-1f, 0f, 0f);
        this.rigidBody.angularVelocity = Vector3.zero;
        this.rigidBody.velocity = Vector3.zero;
        countdownPowerUp = 0f;
        countdownReset = 7.0f;
        count = 0;
        life = 3;
        power = false;
        countEat = 0;
        countSfereEat = 240;
        rigidBody = GetComponent<Rigidbody>();
        //punteggio.text = "Punteggio  = " + count;
        vite.text = "Vite rimaste = " + life;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }
    
    // aggiustare tutti i reward
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bonus")
        {
            if (col.gameObject.tag == "bonus")
            {
                Destroy(col.gameObject);
                count += 100;
                //punteggio.text = "Punteggio  = " + count;
                if (countSfereEat == 0)
                {
                    AddReward(2f);
                }
                else { countSfereEat--; }
            }

            if (col.gameObject.tag == "cerry")
            {
                Destroy(col.gameObject);
                count += 1000;
                //punteggio.text = "Punteggio  = " + count;
            }

            if (col.gameObject.tag == "power")
            {
                Debug.Log("trigger");
                Destroy(col.gameObject);
                power = true;
                countEat = 0;
                countdownPowerUp += 10;
            }
        }
    }

    public void gameOver()
    {
        life--;
        if (life > 0)
        {
            AddReward(-0.3f);
            Debug.Log(GetCumulativeReward());
            vite.text = "Vite rimaste = " + life;
            this.transform.position = new Vector3(0f, 0f, -20.5f);
            reset = true;
        }
        else
        {
            Debug.Log("Perso");
            AddReward(-1.01f);
            EndEpisode();
            Debug.Log(GetCumulativeReward());
        }        
    }
    
    private void Collision(EnemyAi ghost)
    {
        if (!ghost.GetComponent<EnemyAi>().dead)
        {
            if (power && ghost.GetComponent<EnemyAi>().escape)
            {
                countEat++;
                count += countEat * 200;
                //punteggio.text = "Punteggio  = " + count;
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
