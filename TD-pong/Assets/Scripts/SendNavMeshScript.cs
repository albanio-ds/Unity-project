using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SendNavMeshScript : MonoBehaviour
{

    [SerializeField]
    private NavMeshAgent Agent;

    [SerializeField]
    private UnityEvent MyEvent = new UnityEvent();


    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition =  Input.mousePosition; //souris a lecran
            Ray mouseWorldPosition = Camera.main.ScreenPointToRay(mousePosition); //convertir souris en monde unity
            RaycastHit hitInfo; //stocker l info du raycast
            if (Physics.Raycast(mouseWorldPosition, out hitInfo, 100.0f)) 
            {
                Vector3 position = hitInfo.point;
                Debug.Log(position);
                Agent.SetDestination(position); //deplace le navMeshAgent si map baked

                MyEvent.Invoke();
            }
        }
    }
}
