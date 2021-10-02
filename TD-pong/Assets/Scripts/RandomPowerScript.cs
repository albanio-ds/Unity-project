using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerScript : MonoBehaviour
{
    private int nbScripts = 9;
    private bool disabled = false;
    [SerializeField]
    private AudioSource audioSource;
    private MeshRenderer mesh;
    private void Start()
    {
        GameObject.Destroy(transform.gameObject, 5.0f);
        mesh = transform.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("touched");
        if (disabled) return;
        if (other.gameObject.tag == "Ball")
        {
            disabled = true;
            mesh.enabled = false;
            var script = other.gameObject.GetComponent<MovementBall>();
            int i = (int)Random.Range(0.0f, nbScripts);
            //Debug.Log(i);
            if (i == 0)
            {
                script.freezeBall();
            }
            if (i == 1)
            {
                script.invisibleBall();
            }
            if (i == 2)
            {
                script.changeBallScale(0.5f);
            }
            if (i == 3)
            {
                script.changeBallScale(-0.5f);
            }
            if (i == 4)
            {
                script.changeSpeed(1.5f);
            }
            if (i == 5)
            {
                script.changeSpeed(-1f);
            }
            if (i == 6)
            {
                script.changeDirection();
            }
            if (i == 7)
            {
                script.changeDirectionZ();
            }
            if (i == 8)
            {
                script.changeDirectionAndFreeze();
            }
            audioSource.Play();
            Destroy(transform.gameObject, 1f);
        }
    }
}
