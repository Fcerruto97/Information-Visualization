using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class movimento : MonoBehaviour
{
    public Text punteggio, vite;
    public float speed;
    private Rigidbody rigidBody;
    private static int count = 0;
    private static bool power = false;
    private static int life = 3, countEat= 0;

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
            Debug.Log("(" + transform.position.x + "," + transform.position.y + "," + transform.position.z + ")");
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

    /*  IEnumerator loadScene()
      {

          SceneManager.LoadScene("Livello");

      }
      */

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bonus")
        {
            Destroy(col.gameObject);
            count += 100;
            punteggio.text = "Punteggio  = " + count;
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
                col.gameObject.transform.position = new Vector3(0.1f, 0.084f, 0.615f);
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
            powerEat();
        }
    }

    public void powerEat()
    {
        float timer = 10f;
        power = true;
        while (timer > 0)
        {
            if (timer % 2 == 0)
            {

            }
            else
            {

            }        
            timer -= Time.deltaTime; 
            Debug.Log(timer);
        }
        countEat = 0;
        //power = false;
    }    
}