using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMonster : MonoBehaviour
{
    private bool playerOnFloor;
    private EnemyAi monster;
    // Start is called before the first frame update
    void Start()
    {
        playerOnFloor = false;
        monster = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
    }

    // Update is called once per frame
    void Update()
    {
        monster.isOnFloor = playerOnFloor;
    }
    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            playerOnFloor = true;
        } 
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.tag == "Player") {
            playerOnFloor = false;
        }
    }
}
