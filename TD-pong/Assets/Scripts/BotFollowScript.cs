using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotFollowScript : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5.0f;

    private GameObject ball;
    
    // Update is called once per frame
    void Update()
    {
        if (ball == null) return;
        if (ball.transform.position.z  > transform.position.z + 0.5)
        {
            transform.position += Time.deltaTime * Speed * Vector3.forward;
        }
        else if (ball.transform.position.z  < transform.position.z - 0.5)
        {
            transform.position += Time.deltaTime * Speed * Vector3.back;
        }

    }

    public void setBall(GameObject gameBall)
    {
        ball = gameBall;
    }

    public void setSpeed(float spd)
    {
        Speed = spd;
    }
}
