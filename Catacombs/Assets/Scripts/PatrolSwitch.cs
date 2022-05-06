using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolSwitch : MonoBehaviour
{
    private EnemyAi monster;
    public Transform[] patrolPoints, newPatrolPoints;

    void Start()
    {
        monster = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
           for (int i = 0; i < patrolPoints.Length; i++) {
               patrolPoints[i].position = newPatrolPoints[i].position;
           }
           monster.reset();
           Destroy(this.gameObject);
        } 
    }
}