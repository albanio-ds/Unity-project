using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    #region attributs
    public int finale = 0; //-1 pour perdu et 1 pour gagner
    public GameObject[] plateforms;
    #endregion


    public void resetWorld(bool valeur)
    {
        foreach (GameObject element in plateforms)
        {
            element.SetActive(valeur);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        print("objet entrant");
        if (other.tag == "Finish")
        {
            print("fini");
            finale = 1;
            resetWorld(false);
        } else
        {
            if (other.tag == "Dead")
            {
                print("mort");
                resetWorld(true);
                transform.GetComponent<CharacterController>().enabled = false;
                transform.position = new Vector3(-9.846348f, 7.78f, -3.624018f);
                transform.rotation = new Quaternion(0, 90, 0, 0);
                transform.GetComponent<CharacterController>().enabled = true;
            } else
            {
                print("objet entrant autre");
            }
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
        print("test");
    }

    // Update is called once per frame

    
}
