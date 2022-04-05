using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission_01 : MonoBehaviour
{
    public GameObject gameUI;
    private int timer;

    // Start is called before the first frame update
    void Start()
    {
        gameUI = GameObject.Find("prefab_UI");
        timer = 120;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) {
            timer--;
        } else {
            printOpeningDialogue();
        }
    }

    //                   //
    // DIALOGUE PRINTERS //
    //                   //
    void printOpeningDialogue()
    {
        // Print the following text: 
        // "You: Where am I?"
    }

}
