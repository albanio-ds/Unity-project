using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region attributs
    private float score = 0f;
    private int nbDeath = 0;
    private float speed = 2.5f;
    private float jumpPower = 5;
    private float horizontalSpeed = 2f;
    private float verticalSpeed = 2f;
    private bool jumpingPlayer = false;
    private int nbColliders;
    public GameObject cameraPlayer;
    public GameScript gameScript;
    private bool isPlaying = true;
    private Vector3 pos = new Vector3(-9.94f, 7.78f, -3.56f);

    public GameObject[] animations;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying == false)
        {
            reset();
        } else
        {
            movePlayer();
            moveMouse();
            score += (1.5f* Time.deltaTime);
            updateScore();
        }
    }

    public void reset()
    {
        foreach (GameObject element in animations)
        {
            element.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            element.transform.rotation = new Quaternion(0, 0, 0, 0);
            element.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }
        transform.position = new Vector3(-9.94f, 7.78f, -3.56f);
        pos = new Vector3(-9.94f, 7.78f, -3.56f);
        transform.rotation = new Quaternion(0, 90, 0, 0);
        cameraPlayer.transform.rotation = Quaternion.identity;
        cameraPlayer.transform.position = transform.position + new Vector3(0, 1, -5);
        score = 0f;
        nbDeath = 0;
        isPlaying = true;
        jumpingPlayer = false;
        gameScript.ended = false;
    }


    public void updateScore()
    {
        gameScript.button.SetActive(false);
        gameScript.score.GetComponent<UnityEngine.UI.Text>().text = "Score : " + Mathf.Round(score * 100f)/100f ;
        gameScript.falls.GetComponent<UnityEngine.UI.Text>().text = "Chutes : " + nbDeath;
    }

    [System.Obsolete]
    public void movePlayer()
    {
        /*
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        */

        var cameraTr = Camera.current.transform;
        var cameraForward = cameraTr.forward;
        
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += cameraBearing * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position += (-cameraBearing) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += (Quaternion.AngleAxis(-90, Vector3.up) * cameraBearing) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += (Quaternion.AngleAxis(90, Vector3.up) * cameraBearing) * speed * Time.deltaTime;
        }



        if (Input.GetKey(KeyCode.Space))
        {
            if (jumpingPlayer == false && nbColliders > 0)
            {
                jumpingPlayer = true;
                StartCoroutine(jump());
            }
        }
        if (jumpingPlayer == true)
        {
            transform.position += Vector3.up * jumpPower * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.C))
        {
            cameraPlayer.transform.rotation = Quaternion.identity;
            cameraPlayer.transform.position = transform.position + new Vector3(0, 1, -5);

        }
        if (Input.GetKey(KeyCode.R))
        {
            reset();
        }
        
       }

    public void moveMouse()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
                cameraPlayer.transform.Rotate(new Vector3(0, -horizontalSpeed, 0));

            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                cameraPlayer.transform.Rotate(new Vector3(0, horizontalSpeed, 0));
            }

            if (Input.GetAxis("Mouse Y") < 0)
            {
                cameraPlayer.transform.Rotate(new Vector3(verticalSpeed, 0, 0));

            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                cameraPlayer.transform.Rotate(new Vector3(-verticalSpeed, 0, 0));
            }
        }
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
                cameraPlayer.transform.Translate(new Vector3(0.1f, 0, 0));

            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                cameraPlayer.transform.Translate(new Vector3(-0.1f, 0, 0));
            }

            if (Input.GetAxis("Mouse Y") < 0)
            {
                cameraPlayer.transform.Translate(new Vector3(0, 0.1f, 0));

            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                cameraPlayer.transform.Translate(new Vector3(0, -0.1f, 0));
            }
        }

    }
   
    public IEnumerator jump()
    {
        transform.GetComponent<Rigidbody>().mass = 0;
        yield return new WaitForSeconds(1);
        transform.GetComponent<Rigidbody>().mass = 1;
        jumpingPlayer = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dead")
        {
            nbDeath++;
            jumpingPlayer = false;
            print("mort : " + nbDeath + " score : " + score);
            //transform.GetComponent<CharacterController>().enabled = false;
            transform.position = pos;
            transform.rotation = new Quaternion(0, 90, 0, 0);
            //cameraPlayer.transform.rotation = Quaternion.identity;
            //cameraPlayer.transform.position = transform.position + new Vector3(0, 1, -5);
            //transform.GetComponent<CharacterController>().enabled = true;
        } else
        {
            if (other.isTrigger == false) nbColliders++;

            if (other.tag == "Finish")
                {
                    isPlaying = false;
                    gameScript.resetWorld(false, score, false);
                    transform.GetComponent<Player>().enabled = false;
                } else
            {
                if (other.tag == "Checkpoint") pos = other.transform.position + new Vector3(0, 1, 0);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag != "Dead")
        {
            if (other.isTrigger == false) nbColliders--;
        }
    }

}
