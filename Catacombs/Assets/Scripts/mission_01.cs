using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mission_01 : MonoBehaviour
{
    public GameObject gameUI;
    private int timer1;
    private int timer2;
    bool waiting;
    public TextMeshProUGUI dialogueBox;
    public bool bookTextTrigger1;
    public bool bookTextTrigger2;
    public bool bookTextTrigger3;
    public GameObject barrier;

    // Start is called before the first frame update
    void Start()
    {
        gameUI = GameObject.Find("prefab_UI");
        timer1 = 120;
        timer2 = 0;
        waiting = false;
        dialogueBox = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
        dialogueBox.text = "";
        bookTextTrigger1 = false;
        bookTextTrigger2 = false;
        bookTextTrigger3 = false;
        barrier = GameObject.Find("doorBarrier");
    }

    // Update is called once per frame
    void Update()
    {
        // Timer to activate opening dialogue.
        if (timer1 > 0) {
            timer1--;
        } else {
            print_OpeningDialogue();
        }

        // Dialogue that activates when first book is picked up. 
        updateBookTextTriggers();
        if ( (bookTextTrigger3 == true) && (!waiting) ) {
            print_BookDialogue3();
            barrier.SetActive(false);

            setDialogueFade();
        } else if (bookTextTrigger2 == true) {
            // Debug.Log("bookTextTrigger2 == true");
            print_BookDialogue2();

            setDialogueFade();
        } else if (bookTextTrigger1 == true) {
            print_BookDialogue1();

            setDialogueFade();
        }

        // Timer that causes text to disappear after a bit of time.
        if (timer2 > 0) {
            timer2--;
        } else if (waiting) {
            dialogueBox.text = "";
            waiting = false;
        }
    }

    void updateBookTextTriggers()
    {
        bookTextTrigger1 = GameObject.Find("EventSystem").GetComponent<inventoryScript>().firstBookFound;
        // bookTextTrigger2 = GameObject.Find("EventSystem").GetComponent<inventoryScript>().firstBookRead;
        bookTextTrigger3 = GameObject.Find("EventSystem").GetComponent<inventoryScript>().purpleBookFound;
    }

    void setDialogueFade()
    {
        timer2 = 300;
        waiting = true;
    }

    //                   //
    // DIALOGUE PRINTERS //
    //                   //
    void print_OpeningDialogue()
    {
        dialogueBox.text = "You: Where am I? How did I get here?";
    }

    void print_BookDialogue1()
    {
        dialogueBox.text = "You: A book? I wonder what it says...";
    }

    void print_BookDialogue2()
    {
        dialogueBox.text = "You: I need to find that journal.";
    }
    
    void print_BookDialogue3()
    {
        dialogueBox.text = "You: The journal! Let's see what it says...";
    }

    public void print_BarrierText()
    {
        dialogueBox.text = "Complete current objective to advance.";
    }

}
