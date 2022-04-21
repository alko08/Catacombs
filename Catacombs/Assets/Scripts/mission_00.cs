using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class mission_00 : MonoBehaviour
{
    /*********************************************************************\
        Declaring variables
    \*********************************************************************/

    // Objects
    public inventoryScript inventory;
    public mission_01 nextMission;
    public TextMeshProUGUI dialogueBox;

    // Dialogue Stuff
    const int NUM_BOXES = 4;
    public GameObject[] boxes;
    public TextMeshProUGUI[] choices;
    public Button[] buttons;
    public Button exitButton;
    int dialogueRound;
    string prevChoice;
    
    // Timers
    private int timer1;

    // Bools
    bool opening_done;
    bool blytheTalk_done;
    bool notClear;
    bool pause;
    public bool isOpen_dialogue;
    bool blytheTalk_done1;

    // Tasks.
    const int NUM_TASKS = 6;
    string[] tasks;
    
    /*********************************************************************\
        Basic Functions
    \*********************************************************************/

    // Initializing.
    void Start()
    {
        // Things outside of mission_00.
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        nextMission = gameObject.GetComponent<mission_01>();
            nextMission.enabled = false;
        dialogueBox = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            clear();
        
        // Dialogue Choices.
        initiateBoxes();
        dialogueRound = 0;
        prevChoice = "";

        // Tasks.
        initiateTasks();
        
        // Timers.
        timer1 = 120;

        // Bools.
        opening_done = false;
        blytheTalk_done = false;
        pause = false;
        isOpen_dialogue = false;
        blytheTalk_done1 = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Mission starts like mission_01 used to: Confused dialogue after a
        // short timer. 
        if (timer1 > 0) {
            timer1--;
        } else if (!pause) {
            if (!opening_done) {
                print_opening();
            }
            
            else if (notClear) {
                clear();
            }
        } else if ((blytheTalk_done) && (!blytheTalk_done1)) {
            print_blytheTalk2();
        } else if (notClear) {
            clear();
        }
    }

    /*********************************************************************\
        Initialization Helper Functions
    \*********************************************************************/

    void initiateBoxes()
    {
        boxes = new GameObject[NUM_BOXES];
        choices = new TextMeshProUGUI[NUM_BOXES];
        buttons = new Button[NUM_BOXES];
        
        string currBox;
        string currChoice;
        for (int i = 0; i < NUM_BOXES; i++) {
            currBox = "box" + (i + 1).ToString();
                // Debug.Log("Current box: " + currBox);
            boxes[i] = GameObject.Find(currBox);

            currChoice = "choice" + (i + 1).ToString();
            choices[i] = GameObject.Find(currChoice).GetComponent<TextMeshProUGUI>();

            buttons[i] = boxes[i].GetComponent<Button>();
        }

        // Initiating button listeners.
        buttons[0].onClick.AddListener(ButtonClicked_LT);
        buttons[1].onClick.AddListener(ButtonClicked_LB);
        buttons[2].onClick.AddListener(ButtonClicked_RT);
        buttons[3].onClick.AddListener(ButtonClicked_RB);

        // Now that buttons have been individually primed, close the boxes.
        for (int i = 0; i < NUM_BOXES; i++) {
            boxes[i].SetActive(false);
        }

        exitButton = GameObject.Find("exit").GetComponent<Button>();
        exitButton.onClick.AddListener(ButtonClicked_exit);
        exitButton.gameObject.SetActive(false);
    }

    void initiateTasks()
    {
        tasks = new string[NUM_TASKS];

        tasks[0] = "Find someone to talk to.";
    }

    /*********************************************************************\
        Dialogue Functions
    \*********************************************************************/

    void clear()
    {
        dialogueBox.text = "";
        notClear = false;
    }
    
    void print_opening()
    {
        dialogueBox.text = "You: Where am I? How did I get here?";
        inventory.addTask(tasks[0]);
        timer1 = 300;
        opening_done = true;
        notClear = true;
    }

    public void print_blytheTalk1()
    {
        inventory.removeTask(tasks[0]);
        dialogueBox.text = "Giant Bug: Oh hey there! How're you doing?";
        timer1 = 0;
        pause = true;   // Pausing text change to open talking menu.
        notClear = false;   // Setting to false to prevent dialogue from getting erased instantly.
        dialogueRound++;    // Should go to 1.
        openDialogueOptions();
    }

    void print_blytheTalk2()
    {
        dialogueBox.text = "Giant Bug: Look, if you're confused, I'd suggest lookin " +
                           "around. In case ya couldn't tell, we're in Tisch " +
                           "library right now. Read a book!";
        timer1 = 300;
        blytheTalk_done1 = true;
    }

    /*********************************************************************\
        Dialogue Option Functions
    \*********************************************************************/

    // Function that runs dialogue tree. 
    void openDialogueOptions()
    {
        setNonUI(false);
        isOpen_dialogue = true;

        for (int i = 0; i < NUM_BOXES; i++) {
            boxes[i].SetActive(true);
        }
        exitButton.gameObject.SetActive(true);

        choices[0].text = "AAHHHHH! GIANT BUG! AAHHHHH!";
        choices[1].text = "Where am I? Who are you? How are you talking?";
        choices[2].text = "Am I dead?";
        choices[3].text = "What was that thing that just walked by?";
    }

    void closeDialogueOptions()
    {
        setNonUI(true);
        isOpen_dialogue = false;

        for (int i = 0; i < NUM_BOXES; i++) {
            boxes[i].SetActive(false);
        }
        exitButton.gameObject.SetActive(false);

        blytheTalk_done1 = false;
    }

    void ButtonClicked_LT()
    {
        dialogueRound++;
        Debug.Log("LT Pressed. Current Round: " + dialogueRound.ToString());

        if (dialogueRound == 2) {
            dialogueBox.text = "Giant Bug: Hey, hey! Calm down! I don't bite!";
            prevChoice = "LT";
            changeDialogueBoxes();
        }

        // Player said: "AAHHHHH"
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueBox.text = "";
            dialogueRound = 100;
            changeDialogueBoxes();
        }

        // Player said: "How are you talking?"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "";
            dialogueRound = 101;
            changeDialogueBoxes();
        }
    }

    void ButtonClicked_LB()
    {
        dialogueRound++;
        Debug.Log("LB Pressed. Current Round: " + dialogueRound.ToString());

        if (dialogueRound == 2) {
            dialogueBox.text = "Giant Bug: You're asking a lotta questions, friend!";
            prevChoice = "LB";
            changeDialogueBoxes();
        }

        // Player said: "Are you going to answer any of them?"
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueBox.text = "";
            dialogueRound = 200;
            changeDialogueBoxes();
        }

        // Player said: "You're not given a lotta answers!"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "";
            dialogueRound = 201;
            changeDialogueBoxes();
        }
    }

    void ButtonClicked_RT()
    {
        dialogueRound++;
        Debug.Log("RT Pressed. Current Round: " + dialogueRound.ToString());

        if (dialogueRound == 2) {
            dialogueBox.text = "Giant Bug: Haha! Nope!";
            prevChoice = "RT";
            changeDialogueBoxes();
        }

        // Player said: "Then where am I?"
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueBox.text = "";
            dialogueRound = 300;
            changeDialogueBoxes();
        }

        // Player said: "Are you sure?"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "";
            dialogueRound = 301;
            changeDialogueBoxes();
        }
    }

    void ButtonClicked_RB()
    {
        dialogueRound++;
        Debug.Log("RB Pressed. Current Round: " + dialogueRound.ToString());

        if (dialogueRound == 2) {
            dialogueBox.text = "Giant Bug: Oh him? I'm not too sure actually." +
                               "I'd avoid him if you can, though! He bites!";
            prevChoice = "RB";
            changeDialogueBoxes();
        }

        // Player said: "He bites!?"
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueBox.text = "";
            dialogueRound = 400;
            changeDialogueBoxes();
        }

        // Player said: "Did he not see us?"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "";
            dialogueRound = 401;
            changeDialogueBoxes();
        }
    }

    void ButtonClicked_exit()
    {
        // Resetting dialogue.
        dialogueRound = 0;
        prevChoice = "";
        closeDialogueOptions();
    }

    // Toggling non-UI controls. Yoinked straight from inventoryScript.
    void setNonUI(bool NonUI_status)
    {
        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = NonUI_status;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = !NonUI_status;
    }

    // Function used to change dialogue boxes starting on dialogueRound == 2.
    void changeDialogueBoxes() {
        // ROUND 2.
        if (dialogueRound == 2) {
            
            // Giant Bug's Dialogue: "Hey, hey! Calm down! I don't bite!"
            if (prevChoice == "LT") {
                choices[0].text = "AAHHHHH!";
                choices[1].text = "";
                choices[2].text = "How are you talking?";
                choices[3].text = "";
            }

            // Giant Bug's Dialogue: "You're asking a lotta questions, friend!"
            else if (prevChoice == "LB") {
                choices[0].text = "Are you going to answer any of them?";
                choices[1].text = "";
                choices[2].text = "You're not given a lotta answers!";
                choices[3].text = "";
            }

            // Giant Bug's Dialogue: "Haha! Nope!"
            else if (prevChoice == "RT") {
                choices[0].text = "Then where am I?";
                choices[1].text = "";
                choices[2].text = "Are you sure?";
                choices[3].text = "";
            }

            // Giant Bug's Dialogue: "Giant Bug: Oh him? I'm not too sure actually.
            //                        I'd avoid him if you can, though! He bites!"
            else if (prevChoice == "RB") {
                choices[0].text = "He bites!?";
                choices[1].text = "";
                choices[2].text = "Did he not see us?";
                choices[3].text = "";
            }

        }
    }

}
