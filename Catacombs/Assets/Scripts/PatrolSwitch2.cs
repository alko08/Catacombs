using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolSwitch2 : MonoBehaviour
{
    public Transform[] patrolPoints;
    private EnemyAi monster;

    void Start()
    {
        monster = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            for (int i = 0; i < patrolPoints.Length; i++) {
               patrolPoints[i].position = transform.GetChild(i).position;
            }
        } 
    }
}
