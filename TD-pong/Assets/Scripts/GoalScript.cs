using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Ball") return;
        gameManager.ScoreGoal(transform.name, other.gameObject);
    }
}
