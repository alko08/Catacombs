using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

    /*********************************************************************\
        KNOWN BUGS
    \*********************************************************************/

/*****************************************************************************\

    - None :)

\*****************************************************************************/

public class mission_00 : MonoBehaviour
{
    /*********************************************************************\
        Declaring variables
    \*********************************************************************/

    // Objects
    public inventoryScript inventory;
    // public mission_01 nextMission;
    public TextMeshProUGUI dialogueBox;

    // Dialogue Stuff
    const int NUM_BOXES = 4;
    public GameObject[] boxes;
    public TextMeshProUGUI[] choices;
    public Button[] buttons;
    public Button exitButton;
    int dialogueRound;
    string prevChoice;
    int finalDialogueCode;
    
    // Timers
    private int timer1;

    // Bools
    bool opening_done;
    // bool blytheTalk_done;
    bool notClear;
    bool pause;
    public bool isOpen_dialogue;
    // bool blytheTalk_done1;
    bool checker1;

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
            inventory.currMission = 0;
        // nextMission = gameObject.GetComponent<mission_01>();
        //     nextMission.enabled = false;
        dialogueBox = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            clear();
        
        // Dialogue Choices.
        initiateBoxes();
        dialogueRound = 0;
        prevChoice = "";
        finalDialogueCode = 0;
        GameObject.Find("blythe").GetComponent<blythe>().currMission = 0;
        GameObject.Find("exit_door").GetComponent<DoorLocked>().currMission = 0;

        // Tasks.
        initiateTasks();
        
        // Timers.
        timer1 = 120;

        // Bools.
        opening_done = false;
        // blytheTalk_done = false;
        pause = false;
        isOpen_dialogue = false;
        // blytheTalk_done1 = false;
        checker1 = false;
    }

    // FixedUpdate is called once per tick. Therefore timer stays constant 
    // between devices with better frames per second
    void FixedUpdate()
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
        // } else if ((blytheTalk_done) && (!blytheTalk_done1)) {
        //     print_blytheTalk2();
        
        else if (checker1) {
            print_task1Dialogue_2();
        }
        
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
        tasks[1] = "Explore Tisch and find your friend.";
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
        if (!isOpen_dialogue) {
            inventory.removeTask(tasks[0]);
            dialogueBox.text = "Giant Bug: Oh hey there! How're you doing?";
            timer1 = 0;
            pause = true;   // Pausing text change to open talking menu.
            notClear = false;   // Setting to false to prevent dialogue from getting erased instantly.
            dialogueRound = 1;  // Setting dialogue round to start mode.
            openDialogueOptions();
        }
    }

    void print_blytheTalk2()
    {
        dialogueBox.text = "Giant Bug: Look, if you're confused, I'd suggest lookin " +
                           "around. In case ya couldn't tell, we're in Tisch " +
                           "library right now. Read a book!";
        timer1 = 300;
        // blytheTalk_done1 = true;
    }

    void print_task1Dialogue()
    {
        closeDialogueOptions();
        
        timer1 = 240;
        checker1 = true;

        inventory.addTask(tasks[1]);

        Debug.Log("Waiting for task1 stuff to print...");
    }

    void print_task1Dialogue_2()
    {
        dialogueBox.text = "You: I gotta find my friend!";
        // inventory.addTask(tasks[1]);
        timer1 = 120;
        notClear = true;
    }

    public void print_doorMessage()
    {
        dialogueBox.text = "The door is locked. Maybe there's a key...";
        timer1 = 180;
        notClear = true;
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
        choices[3].text = "What's that thing behind the glass?";
    }

    void closeDialogueOptions()
    {
        setNonUI(true);
        isOpen_dialogue = false;

        for (int i = 0; i < NUM_BOXES; i++) {
            boxes[i].SetActive(false);
        }
        exitButton.gameObject.SetActive(false);
        dialogueRound = 0;  // Resetting dialogue round.

        // blytheTalk_done1 = false;

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
            dialogueRound = 2;
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

        // Player said: "Why am I in Tisch?" / "What do you mean I bonked my head?"
        else if ( (finalDialogueCode == 1) || (finalDialogueCode == 2) 
               || (finalDialogueCode == 3) ) {
            dialogueBox.text = "Giant Bug: Well, you were lookin for your " +
                               "friend and someone didn't want you to find them, so they " +
                               "bonked you upside the head!";
            print_task1Dialogue();
        }

        // Player said: "People like me?"
        else if (finalDialogueCode == 4) {
            dialogueBox.text = "Giant Bug: Yup! Don't you remember? You were sneaking " +
                               "around lookin for your friend when someone bonked you on " +
                               "the head!";
            print_task1Dialogue();
        }

        // Player said: "Why is he here?"
        else if (finalDialogueCode == 5) {
            dialogueBox.text = "Giant Bug: He's guarding the lower levels of the library. " +
                               "Someone put him there so people like you don't go snooping around.";
            dialogueRound = 203;
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

        // Player said: "Calm down!? You're a giant bug!"
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueBox.text = "Giant Bug: That's rude! I'm the first person you find " +
                               "after bonking your head and you're treating me like this!?";
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

        // Player said: "Then why is everything so dark and creepy?"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "Giant Bug: Wow, you must've bonked your head real hard earlier. " +
                               "It's all dark and creepy because we're in Tisch Library, obviously!";
            dialogueRound = 202;
            changeDialogueBoxes();
        }

        // Player said: "What's he doing in there?"
        else if ( (dialogueRound == 3) && (prevChoice == "RB") ) {
            dialogueBox.text = "Giant Bug: He's guarding the lower levels of the library. " +
                               "Someone put him there so people like you don't go snooping around.";
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

        // Player said: "Where am I? Who are you? How are you talking?"
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueRound = 1;
            ButtonClicked_LB();
        }

        // Player said: "You're not given a lotta answers!"
        else if ( (dialogueRound == 3) && (prevChoice == "LB") ) {
            dialogueBox.text = "Giant Bug: So impatient... Fine. You're in Tisch Library, I'm " +
                               "Giant Bug, and I can talk because I feel like it.";
            dialogueRound = 301;
            changeDialogueBoxes();
        }

        // Player said: "Are you sure?"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "Giant Bug: Yup! You just bonked your head real hard " +
                               "earlier! That might be why you don't remember anything.";
            dialogueRound = 302;
            changeDialogueBoxes();
        }

        // Player said: "Did he not see us?"
        else if ( (dialogueRound == 3) && (prevChoice == "RB") ) {
            dialogueBox.text = "Giant Bug: Probably not! His eyes aren't all that " +
                               "good, you see. Mostly uses his ears to find things.";
            dialogueRound = 303;
            changeDialogueBoxes();
        }

        // Player said: "What do you mean I bonked my head?"
        else if (finalDialogueCode == 2) {
            dialogueBox.text = "Giant Bug: Well, you were lookin for your " +
                               "friend and someone didn't want you to find them, so they " +
                               "bonked you upside the head!";
            print_task1Dialogue();
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

        // Player said: "(Say nothing and flee)"
        else if ( (dialogueRound == 3) && (prevChoice == "LT") ) {
            dialogueBox.text = "Hey! Where're you going?";
            closeDialogueOptions();
        }

        // Player said: "Not helpful."
        else if ( (dialogueRound == 3) && (prevChoice == "LB") ) {
            dialogueBox.text = "Giant Bug: So impatient... Fine. You're in Tisch Library, I'm " +
                               "Giant Bug, and I can talk because I feel like it.";
            dialogueRound = 401;
            changeDialogueBoxes();
        }

        // Player said: "I don't have time for this... (Leave)"
        else if ( (dialogueRound == 3) && (prevChoice == "RT") ) {
            dialogueBox.text = "Hey! Where're you going?";
            closeDialogueOptions();
        }

        // Player said: "Are you sure we're safe?"
        else if ( (dialogueRound == 3) && (prevChoice == "RB") ) {
            dialogueBox.text = "Giant Bug: Yup! Don't worry, his eyesight " +
                               "isn't all that great, so he mainly finds things by " +
                               "listening.";
            dialogueRound = 403;
            changeDialogueBoxes();
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
                choices[1].text = "Calm down!? You're a giant bug!";
                choices[2].text = "Where am I? Who are you? How are you talking?";
                choices[3].text = "(Say nothing and flee)";
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
                choices[1].text = "Then why is everything so dark and creepy?";
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

        // Giant Bug's Dialogue: "Giant Bug: So impatient... Fine. You're in Tisch Library, I'm " +
        //                       "Giant Bug, and I can talk because I feel like it."
        else if ( (dialogueRound == 101) || (dialogueRound == 301) || (dialogueRound == 401) ) {
            choices[0].text = "Why am I in Tisch?";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";

            finalDialogueCode = 1;
        } 
        
        // Giant Bug's Dialogue: "Giant Bug: Wow, you must've hit your head pretty " +
        //                       "hard to have forgotten that! You're in Tisch Library!"
        else if ( (dialogueRound == 102) || (dialogueRound == 201) || (dialogueRound == 202) ) {
            choices[0].text = "Why am I in Tisch?";
            choices[1].text = "";
            choices[2].text = "What do you mean I bonked my head?";
            choices[3].text = "";

            finalDialogueCode = 2;
        }
        
        // Giant bug's Dialogue: "Giant Bug: That's rude! I'm the first person you find " +
        //                       "after bonking your head and you're treating me like this!?"
        else if ( (dialogueRound == 200) || (dialogueRound == 302) ) {
            choices[0].text = "What do you mean I bonked my head?";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";

            finalDialogueCode = 3;
        }
        
        // Giant Bug's Dialogue: "Giant Bug: He's guarding the lower levels of the library. " +
        //                       "Someone put him there so people like you don't go snooping around."
        else if (dialogueRound == 203) {
            choices[0].text = "People like me?";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";

            finalDialogueCode = 4;
        }

        // Giant Bug's Dialogue: "Giant Bug: Probably not! His eyes aren't all that " +
        //                       "good, you see. Mostly uses his ears to find things."
        else if ( (dialogueRound == 303) || (dialogueRound == 403) || (dialogueRound == 103) ) {
            choices[0].text = "Why is he here?";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";

            finalDialogueCode = 5;
        }

        else {
            closeDialogueOptions();
        }
    }

}
