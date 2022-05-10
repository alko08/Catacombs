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

public class mission_01 : MonoBehaviour
{
    /*********************************************************************\
        Declaring variables
    \*********************************************************************/

    // Objects
    public inventoryScript inventory;
    public TextMeshProUGUI dialogueBox;

    // Dialogue Stuff
    const int NUM_BOXES = 4;
    public GameObject[] boxes;
    public TextMeshProUGUI[] choices;
    public Button[] buttons;
    public Button exitButton;
    int dialogueRound;
        
    // Timer
    int timer;

    // Bools
    bool openingPrinted;
    bool doClear; // If true, dialogue box is cleared when timer reaches 0.
    public bool isOpen_dialogue;
    bool noticedAlready;
    bool doneTalking;
    bool booksFound;
    bool taskAdd1_complete;

    // Tasks
    const int NUM_TASKS = 6;
    string[] tasks;


    // Start is called before the first frame update. Here, we initialize all
    // of our variables to their default states. Objects get assigned here
    // as well. 
    void Start()
    {
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
            inventory.currMission = 1;
        dialogueBox = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
        dialogueBox.text = "";

        initiateBoxes();
        dialogueRound = 0;
        GameObject.Find("blythe").GetComponent<blythe>().currMission = 1;
        GameObject.Find("LockedDoor").GetComponent<DoorLocked>().currMission = 1;

        timer = 120;    // Small delay before opening is printed.

        openingPrinted = false;
        doClear = false;
        isOpen_dialogue = false;
        noticedAlready = false;
        doneTalking = false;
        booksFound = false;
        taskAdd1_complete = false;

        initiateTasks();
    }

    // FixedUpdate is called once per tick. The way we do dialogue is by having
    // a timer that constantly ticks down. Each time it hits 0, this function
    // will do something that affects the dialogue box.
    void FixedUpdate()
    {
        if (timer > 0) {
            timer--;
        } else if (!openingPrinted) {
            print_OpeningDialogue();
        }
        
        else if (doClear) {
            clear();
        }

        if (inventory.numBooks >= 10) {
            booksFound = true;
            doTaskAdd1();
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

        tasks[0] = "Explore Tisch and find your friend.";
        tasks[1] = "Speak to the Giant Bug.";
        tasks[2] = "Find 10 books for the Giant Bug in return for a key.";
        tasks[3] = "Get to the door and keep exploring.";

        inventory.addTask(tasks[0]);
    }

    /*********************************************************************\
        Misc Helper Functions
    \*********************************************************************/

    // Toggles non-UI elements (mainly crosshair stuff). 
    // Pass false to disable (opening UI), pass false to enable (closing UI).
    void setNonUI(bool NonUI_status)
    {
        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = NonUI_status;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = !NonUI_status;
    }

    // TASK ADDERS
    void doTaskAdd()
    {
        closeDialogueOptions();
        inventory.addTask(tasks[2]);

        doneTalking = true;
    }

    void doTaskAdd1()
    {
        if (!taskAdd1_complete) {
            inventory.removeTask(tasks[2]);
            inventory.addTask(tasks[4]);
            taskAdd1_complete = true;
        }
    }

    /*********************************************************************\
        Dialogue Functions
    \*********************************************************************/

    void clear()
    {
        dialogueBox.text = "";
        doClear = false;
    }

    // BASIC DIALOGUE PRINTERS
    void print_OpeningDialogue()
    {
        dialogueBox.text = "You: I wonder what's down here...";
        openingPrinted = true;
        timer = 300;
        doClear = true;
    }

    public void print_noticeBlythe()
    {
        if (!noticedAlready) {
            dialogueBox.text = "You: Is that the Giant Bug?";
            inventory.addTask(tasks[1]);
            timer = 300;
            doClear = true;
            noticedAlready = true;
        }
    }

    public void print_blytheTalk()
    {
        if ( (!doneTalking) && (!isOpen_dialogue) ) {
            inventory.removeTask(tasks[1]);
            dialogueBox.text = "Giant Bug: Hey, it's you again! I hope the monster " +
                           "wasn't too bad!";
            timer = 0;
            doClear = false;
            openDialogueOptions();
        } else if ( (doneTalking) && (!booksFound) ) {
            dialogueBox.text = "Giant Bug: I told you already: Find the books, " +
                               "get the key. Very simple!";
            timer = 180;
            doClear = true;
        } else if ( (doneTalking) && (booksFound) ) {
            dialogueBox.text = "Giant Bug: Wow, thanks for the books! Here's the " +
                               "key! Good lucking exploring!";
            inventory.removeBook("ALL_OF_THEM");
            inventory.addBook("keyring");
            
            timer = 180;
            doClear = true;

            inventory.removeTask(tasks[2]);
            inventory.addTask(tasks[3]);
        }
    }

    public void print_doorMessage()
    {
        dialogueBox.text = "The door is locked. Maybe there's a key...";
        timer = 180;
        doClear = true;
    }

    // DIALOGUE TREE HELPERS
    void openDialogueOptions()
    {
        setNonUI(false);
        isOpen_dialogue = true;

        for (int i = 0; i < NUM_BOXES; i++) {
            boxes[i].SetActive(true);
        }
        exitButton.gameObject.SetActive(true);

        updateDialogueBoxes(/* ROUND: 0 */);
    }

    void closeDialogueOptions()
    {
        setNonUI(true);
        isOpen_dialogue = false;

        for (int i = 0; i < NUM_BOXES; i++) {
            boxes[i].SetActive(false);
        }
        exitButton.gameObject.SetActive(false);
        dialogueRound = 0;

        timer = 240;
        doClear = true;
    }

    /*********************************************************************\
        Button-Press Functions
    \*********************************************************************/

    void ButtonClicked_LT()
    {
        // Player said: "It was terrible! It can't follow me, right?"
        if (dialogueRound == 0) {
            dialogueRound = 100;
            dialogueBox.text = "Giant Bug: Nope! It's not in this room, but you can find " +
                               "it again in the chamber downstairs!";
            updateDialogueBoxes(/* 100 */);
        } 

        // Player said: "Downstairs!? I don't need to go down there, do I?"
        else if ( (dialogueRound == 100) || (dialogueRound == 101) ) {
            dialogueRound = 200;
            dialogueBox.text = "Giant Bug: Well, there are a bunch of cool books " +
                               "down there. If you can bring me 10 of them, I'll " +
                               "give you a key that lets you progress through Tisch.";
            updateDialogueBoxes(/* 200 */);
        }

        // Player said: "Is there a key?"
        else if (dialogueRound == 102) {
            dialogueRound = 202;
            dialogueBox.text = "Giant Bug: Yup! I got one on me, but I'm not " +
                               "gonna just give it up! You have to give me something " +
                               "in return!";
            updateDialogueBoxes(/* 202 */);
        }

        // Player said: "Do you know where they went?"
        else if (dialogueRound == 103) {
            dialogueRound = 0;
            ButtonClicked_RT();
        }

        // Player said: "Fine. I'll get you the books."
        else if ( (dialogueRound == 200) || (dialogueRound == 300) ) {
            dialogueBox.text = "Giant Bug: Awesome! See you in a bit!";
            doTaskAdd();
        }

        // Player said: "What do you want for the key?"
        else if (dialogueRound == 202) {
            dialogueRound = 200;
            dialogueBox.text = "Giant Bug: Well, there're a bunch of cool books downstairs. " +
                               "There's guarded by the monster, but if you can bring " +
                               "10, I'll give you the key.";
            updateDialogueBoxes(/* 200 */);
        }
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_LB()
    {
        // Player said: "How did you get down here?"
        if (dialogueRound == 0) {
            dialogueRound = 101;
            dialogueBox.text = "Giant Bug: Don't worry about it! You should be " +
                               "more concerned about the monster! It's in the room downstairs!";
            updateDialogueBoxes(/* 101 */);
        } 
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_RT() 
    {
        // Player said: "Do you know where my friend went?"
        if (dialogueRound == 0) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: Hmm, well I saw someone going through " +
                               "that exit door behind me. Thing is, the door's " +
                               "locked.";
            updateDialogueBoxes(/* 102 */);
        } 

        // Player said: "What else is downstairs?"
        else if ( (dialogueRound == 100) || (dialogueRound == 101) ) {
            dialogueRound = 200;
            dialogueBox.text = "Giant Bug: Well, there are a bunch of cool books " +
                               "down there. If you can bring me 10 of them, I'll " +
                               "give you a key that lets you progress through Tisch.";
            updateDialogueBoxes(/* 200 */);
        }

        // Player said: "How do I get past it?"
        else if (dialogueRound == 102) {
            dialogueRound = 202;
            dialogueBox.text = "Giant Bug: Well, I got a key on me, but I'm " +
                               "not gonna just give them up! You gotta get me " +
                               "something in return!";
            updateDialogueBoxes(/* 202 */);
        }

        // Player said: "What if they got eaten by the monster?"
        else if (dialogueRound == 103) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: I wouldn't worry about that! I saw " +
                               "them go through that door to my right. Thing is, " +
                               "the door's locked.";
            updateDialogueBoxes(/* 102 */);
        }

        // Player said: "I don't know, seems dangerous with the monster " +
        //              "down there..."
        else if (dialogueRound == 200) {
            dialogueRound = 300;
            dialogueBox.text = "Giant Bug: You already got past the monster " +
                               "once. Doing it again should be a walk in the " +
                               "park!";
            updateDialogueBoxes(/* 300 */);
        }

        // Player said: "What if I just took it by force?"
        else if (dialogueRound == 202) {
            dialogueRound = 200;
            dialogueBox.text = "Giant Bug: Don't make me laugh! I could take " +
                               "you any day of the week! Now, if you want the key, " +
                               "fetch me 10 books from downstairs.";
            updateDialogueBoxes(/* 200 */);
        }
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_RB() 
    {
        // Player said: "What do I do from here?"
        if (dialogueRound == 0) {
            dialogueRound = 103;
            dialogueBox.text = "Giant Bug: You're asking me? It was your idea " +
                               "to come down here in the first place! I don't " +
                               "know, maybe look for your friend?";
            updateDialogueBoxes(/* 103 */);
        } 
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_exit()
    {
        // Resetting dialogue.
        closeDialogueOptions();
    }

    /*********************************************************************\
        Dialogue Tree Updater
    \*********************************************************************/

    void updateDialogueBoxes()
    {
        // ROUND 0.
        if (dialogueRound == 0) {
            choices[0].text = "It was terrible! It can't follow me, right?";
            choices[1].text = "How did you get down here?";
            choices[2].text = "Do you know where my friend went?";
            choices[3].text = "What do I do from here?";
        }

        // ROUND 1.
        else if (dialogueRound == 100) {
            choices[0].text = "Downstairs!? I don't need to go down there, do I?";
            choices[1].text = "";
            choices[2].text = "What else is downstairs?";
            choices[3].text = "";
        }
        else if (dialogueRound == 101) {
            choices[0].text = "Why should I care? I don't need to go down there, do I?";
            choices[1].text = "";
            choices[2].text = "What else is down there?";
            choices[3].text = "";
        }
        else if (dialogueRound == 102) {
            choices[0].text = "Is there a key?";
            choices[1].text = "";
            choices[2].text = "How do I get past it?";
            choices[3].text = "";
        }
        else if (dialogueRound == 103) {
            choices[0].text = "Do you know where they went?";
            choices[1].text = "";
            choices[2].text = "What if they got eaten by the monster?";
            choices[3].text = "";
        }

        // ROUND 2.
        else if (dialogueRound == 200) {
            choices[0].text = "Fine. I'll get you the books.";
            choices[1].text = "";
            choices[2].text = "I don't know, seems dangerous with the monster " +
                              "down there...";
            choices[3].text = "";
        }
        else if (dialogueRound == 202) {
            choices[0].text = "What do you want for the key?";
            choices[1].text = "";
            choices[2].text = "What if I just took it by force?";
            choices[3].text = "";
        }

        // ROUND 3.
        else if (dialogueRound == 300) {
            choices[0].text = "Fine. I'll get you the books.";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";
        }
    }
}
