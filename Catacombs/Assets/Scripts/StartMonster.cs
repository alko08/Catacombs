using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMonster : MonoBehaviour
{
    private int count;
    private EnemyAi monster;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        monster = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
    }

    // Update is called once per frame
    void Update()
    {
        monster.isOnFloor = count == 1;
    }
    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            count++;
        } 
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.tag == "Player") {
            count--;
        }
    }
}
