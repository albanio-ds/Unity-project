using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Input;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5.0f;

    [SerializeField]
    private string upName = "Up";
    [SerializeField]
    private string downName = "Down";
    // Update is called once per frame
    void Update()
    {
     if (Input.GetButton(upName))
        {
            transform.position += Time.deltaTime * Speed * Vector3.forward;
        }

        if (Input.GetButton(downName))
        {
            transform.position += Time.deltaTime * Speed * Vector3.back;
        }

    }

}
