using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class PacmanAgent : Agent
{
    public Text punteggio, vite, sfere;
    public float speed;
    private Rigidbody rigidBody;
    private static int count = 0;
    private static bool power = false;
    private static int life = 3, countEat = 0, countSfereEat = 240;
   

    private Vector3 startingposition=new Vector3(0.1f, 0.084f, 0.615f);
    
    // Start is called before the first frame update
    public override void Initialize()
    {
        rigidBody = GetComponent<Rigidbody>();

        //verificare posizione inziale
        transform.position=startingposition;
    }

    public override void OnEpisodeBegin()
    {

        Reset(); //da aggiustare
    }

    public override void Heuristic(float[] actionsOut)
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

        //Teletrasporto nei corridoi
        Vector3 move = new Vector3(h, 0.0f, v);

        rigidBody.AddForce(move * speed);

        if (transform.position.x > 9.4f)
        {
            transform.position = new Vector3(-8.5f, 0.084f, 0.69f);
        }

        if (transform.position.x < -8.9f)
        {
            transform.position = new Vector3(9.0f, 0.084f, 0.69f);
        }
    }


   //da capire
    public override void CollectObservations(VectorSensor sensor)
    {
        // sensor.AddObservation(target.localPosition);
        //sensor.AddObservation(this.transform.localPosition);

        sensor.AddObservation(this.transform.position);
        

    }
    //da modificare
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 move = Vector3.zero;
        move.x = vectorAction[0];

        if (vectorAction[1] == 2)
        {
            move.z = 1;
        }
        else
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
    }

    public void Reset()
    {

        //verificare settaggio paremtri iniziale
        this.transform.localPosition = new Vector3(0.1f, 0.084f, 0.615f);
        this.rigidBody.angularVelocity = Vector3.zero;
        this.rigidBody.velocity = Vector3.zero;
        count = 0;
        power = false;
        life = 3;
        countEat = 0;
        countSfereEat = 240;

    }

    private void FixedUptade()
    {
        //qualcosa da aggiungere sicuro video minuto 8
        // fare qualcosa prima di chiamare il request decision
        RequestDecision();

    }

    // aggiustare tutti i reward
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bonus")
        {
            AddReward(0.1f);
            Debug.Log("Pallina "+ GetCumulativeReward());
            Destroy(col.gameObject);
            count += 100;
            punteggio.text = "Punteggio  = " + count;
            countSfereEat--;
            sfere.text = "" + countSfereEat;
        }

        if (col.gameObject.tag == "cerry")
        {
            AddReward(0.5f);
            Debug.Log("Cerry "+GetCumulativeReward());
            Destroy(col.gameObject);
            count += 1000;
            punteggio.text = "Punteggio  = " + count;
        }

        if (col.gameObject.tag == "ghost")
        {
            //Debug.Log(col.gameObject.GetComponent<Material>().name);
            if (power)
            {
                AddReward(-0.5f);
                Debug.Log("Fantasma "+GetCumulativeReward());
                countEat++;
                col.gameObject.transform.position = new Vector3(0.1f, 0.084f, 0.615f);
                count += countEat * 200;
                punteggio.text = "Punteggio  = " + count;
            }
            else
            {
                
                gameOver();

            }
        }

        if (col.gameObject.tag == "power")
        {
            Destroy(col.gameObject);
            powerEat();
        }


    }
    public void gameOver()
    {
        // aggiustare premi e punizioni
        life--;
        if (life > 0)
        {
            AddReward(-0.3f);
            Debug.Log(GetCumulativeReward());
            vite.text = "Vite rimaste = " + life;
            transform.position = new Vector3(0.002f, 0.084f, -6.299f);
        }
        else
        {
            Debug.Log("Perso");
            //SceneManager.LoadScene("Game Over");
            AddReward(-1.01f);
            EndEpisode();
            Debug.Log(GetCumulativeReward());
        }
    }

    public void powerEat()
    {

        power = true;
        //StartCoroutine(waitPower());
        countEat = 0;
        power = false;
    }

}
