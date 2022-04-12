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
    bool trigDone0, trigDone1, trigDone2, trigDone3;
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
        trigDone0 = false;
        trigDone1 = false;
        trigDone2 = false;
        trigDone3 = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Timer to activate opening dialogue.
        if (timer1 > 0) {
            timer1--;
        } else {
            if (!trigDone0) {
                print_OpeningDialogue();
                if (!waiting) {
                setDialogueFade();
                }
            }
        }

        // Dialogue that activates when first book is picked up. 
        updateBookTextTriggers();
        if ( (bookTextTrigger3 == true) && (!trigDone3) ) {
            print_BookDialogue3();
            barrier.SetActive(false);

            if (!waiting) {
                setDialogueFade();
            }
        } else if ( (bookTextTrigger2 == true) && (!trigDone2) ) {
            // Debug.Log("bookTextTrigger2 == true");
            print_BookDialogue2();

            if (!waiting) {
                setDialogueFade();
            }
        } else if ( (bookTextTrigger1 == true) && (!trigDone1) ) {
            print_BookDialogue1();

            if (!waiting) {
                setDialogueFade();
            }
        }

        // Timer that causes text to disappear after a bit of time.
        if (timer2 > 0) {
            timer2--;
            // if (timer2 % 60 == 0) {
            //     Debug.Log("tick");
            // }
        } else if (waiting) {
            setTrigDone();
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
        Debug.Log("tickDown invoked");
        
        timer2 = 600;
        waiting = true;
    }

    void setTrigDone()
    {
        if (dialogueBox.text == "You: Where am I? How did I get here?") {
            trigDone0 = true;
        } else if (dialogueBox.text == "You: A book? I wonder what it says...") {
            trigDone1 = true;
        } else if (dialogueBox.text == "You: I need to find that journal.") {
            trigDone2 = true;
        } else if (dialogueBox.text == "You: The journal! Let's see what it says...") {
            trigDone3 = true;
        }
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
