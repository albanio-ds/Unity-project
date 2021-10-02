using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCamera : MonoBehaviour
{

    public TapToPlace tapToPlace;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Scored")
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            Destroy(other.gameObject);
            tapToPlace.addPoints(other.name);
            //other.enabled = false;
           // GameObject.DestroyImmediate(other.gameObject);
        }
    }
}
