using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

    /*********************************************************************\
        KNOWN BUGS
    \*********************************************************************/

/*****************************************************************************\

    - Dialogue choices don't update properly.

\*****************************************************************************/

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
                Debug.Log("Calling print_opening()");
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
        dialogueRound = 1;  // Setting dialogue round to start mode.
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
        Debug.Log("opening dialogue stuff");
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

        timer1 = 60;
        notClear = true;
    }

    void ButtonClicked_LT()
    {
        if (choices[0].text != "") {
        
        dialogueRound++;
        Debug.Log("LT Pressed. Current Round: " + dialogueRound.ToString());

        if (dialogueRound == 2) {
            dialogueBox.text = "Giant Bug: Hey, hey! Calm down! I don't bite!";
            prevChoice = "LT";
            changeDialogueBoxes();
        }

        // Player said: "AAHHHHH"
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueBox.text = "Giant Bug: Okay, now you're just being rude.";
            dialogueRound = 100;
            changeDialogueBoxes();
        }

        // Player said: "Are you going to answer any of them?"
        else if ( (dialogueRound == 3) && (prevChoice == "LB") ) {
            dialogueBox.text = "Giant Bug: So impatient... Fine. You're in Tisch Library, I'm " +
                               "Giant Bug, and I can talk because I feel like it.";
            dialogueRound = 101;
            changeDialogueBoxes();
        }

        // Player said: "Then where am I?"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "Giant Bug: Wow, you must've hit your head pretty " +
                               "hard to have forgotten that! You're in Tisch Library!";
            dialogueRound = 102;
            changeDialogueBoxes();
        }

        // Player said: "He bites!?"
        else if ( (dialogueRound == 3) && (prevChoice == "RB") ) {
            dialogueBox.text = "Giant Bug: Haha, yup! Don't worry, his eyesight " +
                               "isn't all that great, so he mainly finds things by " +
                               "listening.";
            dialogueRound = 103;
            changeDialogueBoxes();
        }

        else {
            closeDialogueOptions();
        }

        }
    }

    void ButtonClicked_LB()
    {
        if (choices[1].text != "") {
        
        dialogueRound++;
        Debug.Log("LB Pressed. Current Round: " + dialogueRound.ToString());

        if (dialogueRound == 2) {
            dialogueBox.text = "Giant Bug: You're asking a lotta questions, friend!";
            prevChoice = "LB";
            changeDialogueBoxes();
        }

        // Player said: ""
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueBox.text = "";
            dialogueRound = 200;
            changeDialogueBoxes();
        }

        // Player said: "I'm asking because I don't remember I got here."
        else if ( (dialogueRound == 3) && (prevChoice == "LB") ) {
            dialogueBox.text = "Giant Bug: Oh right! You bonked you're head real " +
                               "hard earlier. Lemme fill you in: you're in Tisch Library!";
            dialogueRound = 201;
            changeDialogueBoxes();
        }

        // Player said: ""
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "";
            dialogueRound = 202;
            changeDialogueBoxes();
        }

        // Player said: "What's he doing in there?"
        else if ( (dialogueRound == 3) && (prevChoice == "RB") ) {
            dialogueBox.text = "Giant Bug: He's guarding the lower levels of the library. " +
                               "Someone put him there so people don't go around snooping.";
            dialogueRound = 203;
            changeDialogueBoxes();
        }

        else {
            closeDialogueOptions();
        }

        }
    }

    void ButtonClicked_RT()
    {
        if (choices[2].text != "") {
        
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

        // Player said: ""
        else if ( (dialogueRound == 3) && (prevChoice == "LB") ) {
            dialogueBox.text = "";
            dialogueRound = 301;
            changeDialogueBoxes();
        }

        // Player said: "Are you sure?"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "";
            dialogueRound = 302;
            changeDialogueBoxes();
        }

        // Player said: "I don't have time for this... (Leave)"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "Seeya!";
            closeDialogueOptions();
        }

        else {
            closeDialogueOptions();
        }

        }
    }

    void ButtonClicked_RB()
    {
        if (choices[0].text != "") {
        
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

        // Player said: "What's he doing in there?"
        else if ( (dialogueRound == 3) && (prevChoice == "LB") ) {
            dialogueBox.text = "";
            dialogueRound = 401;
            changeDialogueBoxes();
        }

        // Player said: "Did he not see us?"
        else if ( (dialogueRound == 3) && (prevChoice == "RB") ) {
            dialogueBox.text = "";
            dialogueRound = 402;
            changeDialogueBoxes();
        }

        // Player said: "Are we safe here?"
        // Player said: "I don't have time for this... (Leave)"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "Seeya!";
            closeDialogueOptions();
        }

        else {
            closeDialogueOptions();
        }

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
        
        Debug.Log("Changing dialogue options...");
        
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
                choices[1].text = "I'm asking because I don't remember I got here.";
                choices[2].text = "You're not given a lotta answers!";
                choices[3].text = "Not helpful.";
            }

            // Giant Bug's Dialogue: "Haha! Nope!"
            else if (prevChoice == "RT") {
                choices[0].text = "Then where am I?";
                choices[1].text = "";
                choices[2].text = "Are you sure?";
                choices[3].text = "I don't have time for this... (Leave)";
            }

            // Giant Bug's Dialogue: "Giant Bug: Oh him? I'm not too sure actually.
            //                        I'd avoid him if you can, though! He bites!"
            else if (prevChoice == "RB") {
                choices[0].text = "He bites!?";
                choices[1].text = "What's he doing in there?";
                choices[2].text = "Did he not see us?";
                choices[3].text = "Are we safe here?";
            }

        }

        // ROUND 3.

        // Giant Bug's Dialogue: "Giant Bug: Okay, now you're just being rude."
        else if (dialogueRound == 100) {

        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 101) {

        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 102) {

        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 103) {
            
        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 201) {

        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 202) {
            
        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 203) {

        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 300) {
            
        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 301) {

        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 302) {

        }

        // Giant Bug's Dialogue: 
        else if (dialogueRound == 303) {

        }
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 400) {
            
        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 401) {

        } 
        
        // Giant Bug's Dialogue: 
        else if (dialogueRound == 402) {
            
        }

        else {
            closeDialogueOptions();
        }
    }

}
