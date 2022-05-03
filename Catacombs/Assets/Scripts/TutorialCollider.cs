using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    public Tutorial text;
    public bool isEnd;

    private bool player;

    // Start is called before the first frame update
    void Start()
    {
        player = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd) {
            text.endTutorial = player;
        } else {
            text.nearDesk = player;
        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            player = true;
        } 
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.tag == "Player") {
            player = false;
        }
    }
}
