using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVision : MonoBehaviour
{
    private bool playerInSight;
    private EnemyAi monster;
    // Start is called before the first frame update
    void Start()
    {
        playerInSight = false;
        monster = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
    }

    // Update is called once per frame
    void Update()
    {
        monster.isInSight = playerInSight;
    }
    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            playerInSight = true;
        } 
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.tag == "Player") {
            playerInSight = false;
        }
    }
}
