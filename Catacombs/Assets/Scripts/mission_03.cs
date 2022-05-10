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

public class mission_03 : MonoBehaviour
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
    bool doWeirdPrint;
    bool task2_added;
    bool weirdPrinted;

    // Tasks
    const int NUM_TASKS = 6;
    string[] tasks;


    // Start is called before the first frame update. Here, we initialize all
    // of our variables to their default states. Objects get assigned here
    // as well. 
    void Start()
    {
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        dialogueBox = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
        dialogueBox.text = "";

        initiateBoxes();
        dialogueRound = 0;
        GameObject.Find("blythe").GetComponent<blythe>().currMission = 3;

        timer = 120;    // Small delay before opening is printed.

        openingPrinted = false;
        doClear = false;
        isOpen_dialogue = false;
        doWeirdPrint = false;
        task2_added = false;
        weirdPrinted = false;

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
        } else if ( (doWeirdPrint) && (!weirdPrinted) ){
            print_WeirdText();
        }
        
        else if (doClear) {
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

        tasks[0] = "Explore the Catacombs";
        tasks[1] = "Talk to the giant bug.";
        tasks[2] = "Get the key at the end of the tunnel";

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

        if (!task2_added) {
            inventory.addTask(tasks[2]);
            task2_added = true;
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
        dialogueBox.text = "You: Finally, a room with actual lights.";
        openingPrinted = true;
        timer = 300;
        doClear = true;
    }

    public void print_noticeBlythe()
    {
        dialogueBox.text = "You: The bug! Thank goodness they're here.";
        inventory.addTask(tasks[1]);
        timer = 300;
        doClear = true;
    }

    public void print_blytheTalk()
    {
        if ( (!inventory.hasKey) && (!isOpen_dialogue) ) {
            inventory.removeTask(tasks[1]);
            dialogueBox.text = "Giant Bug: Howdy! It's pretty creepy down here, isn't it?";
            timer = 0;
            doClear = false;
            openDialogueOptions();
        } else {
            dialogueBox.text = "Giant Bug: Nice! You got the key!";
            timer = 300;
            doClear = true;
        }
    }

    public void print_doorMessage()
    {
        dialogueBox.text = "The door is locked. Maybe there's a key...";
        timer = 180;
        doClear = true;
    }

    public void setWeirdTimer()
    {
        Debug.Log("Setting timer...");

        timer = 300;
        doWeirdPrint = true;
    }

    void print_WeirdText()
    {
        dialogueBox.text = "You: What is this place?";
        weirdPrinted = true;
        timer = 300;
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
        // Player said: "Yeah, what's up with that?"
        if (dialogueRound == 0) {
            dialogueRound = 100;
            dialogueBox.text = "Giant Bug: Well, if you couldn't tell, we're in " +
                               "the catacombs beneath Tisch! It's full of bones " +
                               "and secrets!";
            updateDialogueBoxes(/* 100 */);
        }

        // Player said: "Charming... Is my friend down here?"
        else if (dialogueRound == 100) {
            dialogueRound = 200;
            dialogueBox.text = "Giant Bug: Probably! My guess is that you'll " +
                               "find out where they are after you get the key to " +
                               "that door on the left!";
            updateDialogueBoxes(/* 200 */);
        }

        // Player said: "Lovely. How do I find my friend down here?"
        else if (dialogueRound == 101) {
            dialogueRound = 200;
            dialogueBox.text = "Giant Bug: I'd start by finding the key to that " +
                               "door on the left! You can probably find answers " +
                               "from there!";
            updateDialogueBoxes(/* 200 */);
        }

        // Player said: "I'm guessing the monster's down there?"
        else if (dialogueRound == 102) {
            dialogueRound = 202;
            dialogueBox.text = "Giant Bug: Yup! It's guarding a key " +
                               "at the end of that tunnel to your right!";
            updateDialogueBoxes(/* 202 */);
        }

        // Player said: "That's awful! Is the monster still around?"
        else if (dialogueRound == 103) {
            dialogueRound = 202;
            dialogueBox.text = "Giant Bug: Yup! Not in here though. It's in the " +
                               "room past that tunnel on your right, guarding " +
                               "a key.";
            updateDialogueBoxes(/* 202 */);
        }

        // Player said: "And where is the key?"
        else if (dialogueRound == 200) {
            dialogueRound = 300;
            dialogueBox.text = "Giant Bug: Follow the tunnel to your right. It's " +
                               "a big room at the very end!";
            updateDialogueBoxes(/* 300 */);
        }

        // Player said: "I'm guessing I need that key?"
        else if (dialogueRound == 202) {
            dialogueRound = 300;
            dialogueBox.text = "Giant Bug: You're a good guesser! Find the key " +
                               "and you'll open the door!";
            updateDialogueBoxes(/* 301 */);
        }

        // Player said: "I will! See you in a bit!"
        else if (dialogueRound == 212) {
            dialogueBox.text = "Good luck!";
            doTaskAdd();
        }
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_LB()
    {
        // Player said: "I thought this was Tisch. Where are we?"
        if (dialogueRound == 0) {
            dialogueRound = 101;
            dialogueBox.text = "Giant Bug: You've never heard of this place? " +
                               "We're in the catacombs beneath Tisch!";
            updateDialogueBoxes(/* 101 */);
        } 
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_RT() 
    {
        // Player said: "Let me guess. There's a door and a key?"
        if (dialogueRound == 0) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: Yup! If you go down that path to " +
                               "your right, you'll find the room with the key.";
            updateDialogueBoxes(/* 102 */);
        } 

        // Player said: "That's horrifying. Is the monster around?"
        else if (dialogueRound == 100) {
            dialogueRound = 202;
            dialogueBox.text = "Giant Bug: Yup! Not in here though. It's in the " +
                               "room past that tunnel on your right, guarding " +
                               "a key.";
            updateDialogueBoxes(/* 202 */);
        }

        // Player said: "That would explain the bones... Whose are those, by the way?"
        else if (dialogueRound == 101) {
            dialogueRound = 103;
            dialogueBox.text = "Giant Bug: They belonged to other folks " +
                               "who came by lookin for answers! Seems like the " +
                               "monster got them first!";
            updateDialogueBoxes(/* 103 */);
        }

        // Player said: "Anything else I need to know before heading over?"
        else if (dialogueRound == 102) {
            dialogueRound = 212;
            dialogueBox.text = "Giant Bug: Well, the monster's down there, but " +
                               "you could've guessed that. It's pretty angry" +
                               "that it hasn't caught you yet, so be careful!";
            updateDialogueBoxes(/* 212 */);
        }

        // Player said: "Well it's not going to catch me! Where do I need to go?"
        else if (dialogueRound == 103) {
            dialogueRound = 213;
            dialogueBox.text = "Giant Bug: You've got a great attitude! Head down that " +
                               "tunnel on the right and find the key that'll get you " +
                               "past the door to your left.";
            updateDialogueBoxes(/* 213 */);
        }

        // Player said: "This place is huge! How am I supposed to find the key?"
        else if (dialogueRound == 200) {
            dialogueRound = 300;
            dialogueBox.text = "Giant Bug: Follow the tunnel to your right. It's " +
                               "a big room at the very end!";
            updateDialogueBoxes(/* 300 */);
        }

        // Player said: "Sounds good. I'll be back with in a bit."
        else if (dialogueRound == 202) {
            dialogueBox.text = "Giant Bug: Awesome! Good luck!";
            doTaskAdd();
        }

        // Player said: "I will! See you in a bit!"
        else if (dialogueRound == 212) {
            dialogueBox.text = "Giant Bug: Don't doubt yourself! You got this!";
            doTaskAdd();
        }
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_RB() 
    {
        // Player said: "Why are there bones everywhere?"
        if (dialogueRound == 0) {
            dialogueRound = 103;
            dialogueBox.text = "Giant Bug: Oh, those? They belonged to other folks " +
                               "who came by lookin for answers! Seems like the " +
                               "monster got them first!";
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
            choices[0].text = "Yeah, what's up with that?";
            choices[1].text = "I thought this was Tisch. Where are we?";
            choices[2].text = "Let me guess. There's a door and a key?";
            choices[3].text = "Why are there bones everywhere?";
        }

        // ROUND 1. 
        else if (dialogueRound == 100) {
            choices[0].text = "Charming... Is my friend down here?";
            choices[1].text = "";
            choices[2].text = "That's horrifying. Is the monster around?";
            choices[3].text = "";
        }
        else if (dialogueRound == 101) {
            choices[0].text = "Lovely. How do I find my friend down here?";
            choices[1].text = "";
            choices[2].text = "That would explain the bones... Whose are those, by the way?";
            choices[3].text = "";
        }
        else if (dialogueRound == 102) {
            choices[0].text = "I'm guessing the monster's down there?";
            choices[1].text = "";
            choices[2].text = "Anything else I need to know before heading over?";
            choices[3].text = "";
        }
        else if (dialogueRound == 103) {
            choices[0].text = "That's awful! Is the monster still around?";
            choices[1].text = "";
            choices[2].text = "Well it's not going to catch me! Where do I need to go?";
            choices[3].text = "";
        }

        // ROUND 2.
        else if (dialogueRound == 200) {
            choices[0].text = "And where is the key?";
            choices[1].text = "";
            choices[2].text = "This place is huge! How am I supposed to find the key?";
            choices[3].text = "";
        }
        else if (dialogueRound == 202) {
            choices[0].text = "I'm guessing I need that key?";
            choices[1].text = "";
            choices[2].text = "Sounds good. I'll be back with in a bit.";
            choices[3].text = "";
        }
        else if (dialogueRound == 212) {
            choices[0].text = "I will! See you in a bit!";
            choices[1].text = "";
            choices[2].text = "That doesn't sound good. Let's hope I can get past it...";
            choices[3].text = "";
        }
        else if (dialogueRound == 213) {
            choices[0].text = "Sounds good! See you in a bit!";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";
            dialogueRound = 212;
        }
    }
}
