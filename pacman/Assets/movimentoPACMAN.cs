using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class movimentoPACMAN : MonoBehaviour
{
    public Text punteggio, vite, sfere;
    public float speed;
    private Rigidbody rigidBody;
    private static int count = 0;
    bool power = false;
    private static int life = 3, countEat= 0, countSfereEat = 240;

    float countdownPowerUp = 10;
    public Text powerUp;

    // Start is called before the first frame update
    void Start()
    { 
        rigidBody = GetComponent<Rigidbody>();
        punteggio.text = "Punteggio  = " + count;
        vite.text = "Vite rimaste = " + life;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

    }

    // Update is called once per frame
    void Update()
    {
        if (power) { StartCoroutine(PowerEat()); }

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

        if (transform.position.x > 9.4f )
        {
            transform.position = new Vector3(-8.5f, 0.084f, 0.69f);
        }

        if (transform.position.x < -8.9f)
        {
            transform.position = new Vector3(9.0f, 0.084f, 0.69f);
        }
    }

    public void gameOver()
    {
        life--;
        if (life > 0)
        {
            vite.text = "Vite rimaste = " + life;
            transform.position = new Vector3(0.002f, 0.084f, -6.299f);
        }
        else
        {
            Debug.Log("Perso");
            SceneManager.LoadScene("Game Over");
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bonus")
        {
            Destroy(col.gameObject);
            count += 100;
            punteggio.text = "Punteggio  = " + count;
            countSfereEat--;
            sfere.text = "" + countSfereEat;
        }

        if (col.gameObject.tag == "cerry")
        {
            Destroy(col.gameObject);
            count += 1000;
            punteggio.text = "Punteggio  = " + count;
        }

        if (col.gameObject.tag == "ghost")
        {
            if (power)
            {
                countEat++;
                col.gameObject.transform.position = new Vector3(0f, 0.084f, 1f);
                count += countEat*200;
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
            power = true;
            countEat = 0;
            powerUp.text = "true";
        }
    }
    
    public IEnumerator PowerEat()
    {
        if (countdownPowerUp >= 0)
        {            
            countdownPowerUp -= Time.deltaTime;
        }
        else
        {            
            countdownPowerUp = 10;
            power = false;
            powerUp.text = "false";
        }
        yield return null;
    }

    /*public override void OnEpisodeBegin()
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
    }*/
}