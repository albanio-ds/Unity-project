using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBall : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 Direction;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float Speed = 5.0f;
    private bool isFrozen = false;
    private bool isInvisible = false;
    //mask speed audio source wall/ player audioclip, animator
    void Start()
    {
       
        if (Random.Range(0.0f, 2.0f) < 1) Direction = Vector3.right;
        else
        {
            Direction = Vector3.left;
        }
    }

    public void changeSpeed(float speedNew)
    {
        Speed += speedNew;
        Debug.Log("speed new" + speedNew);
    }

    public void changeDirection()
    {
        Direction.x = -Direction.x;
        Debug.Log("direction");
    }

    public void changeDirectionZ()
    {
        Direction.z = -Direction.z;
        Debug.Log("direction z");
    }

    private IEnumerator changeDirectionAndFreezeSet(float rdm)
    {
        isFrozen = true;
        Debug.Log("freeze" + rdm);
        yield return new WaitForSeconds(rdm);
        if (Random.Range(0, 1.0f) < 0.5f) changeDirection();
        if (Random.Range(0, 1.0f) < 0.5f) changeDirectionZ();
        isFrozen = false;
    }

    public void changeDirectionAndFreeze()
    {
        if (isFrozen) return;
        StartCoroutine(changeDirectionAndFreezeSet(Random.Range(0.2f, 3.0f)));
    }
    private IEnumerator freezeBallSet(float rdm)
    {
        isFrozen = true;
        Debug.Log("freeze"+rdm);
        yield return new WaitForSeconds(rdm);
        isFrozen = false;
    }

    public void freezeBall()
    {
        if (isFrozen) return;
        StartCoroutine(freezeBallSet(Random.Range(0.2f, 3.0f)));
    }

    public void invisibleBall()
    {
        if (isInvisible) return;
        StartCoroutine(invisibleBallSet(Random.Range(0.3f, 1.5f)));
    }

    IEnumerator invisibleBallSet(float rdm)
    {
        isInvisible = true;
        Debug.Log("invisible"+rdm);
        transform.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(rdm);
        transform.GetComponent<MeshRenderer>().enabled = true;
        isInvisible = false;
    }

    public void changeBallScale(float n)
    {
        transform.localScale += new Vector3(n, n, n);
        Debug.Log("scale " + n);
    }



    IEnumerator StartBall()
    {
        yield return new WaitForSeconds(3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen) return;
        transform.position += Time.deltaTime*Direction*Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.tag == "Player")
        {
            Direction.x = -Direction.x;
            Direction.z = Random.Range(-1f, 1f);
            Speed += 1.1f;
            audioSource.Play();
            //Debug.Log(Speed);
        } if (collision.gameObject.tag == "Wall") 
        {
            Direction.z = -Direction.z;
            audioSource.Play();
        }
        //1<<coll.gobj.layer
    }
}
