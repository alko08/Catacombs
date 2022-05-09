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

public class mission_02 : MonoBehaviour
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
    bool task2_added;
    bool task3_added;

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
        GameObject.Find("blythe").GetComponent<blythe>().currMission = 2;
        GameObject.Find("exit_door").GetComponent<DoorLocked>().currMission = 2;

        timer = 120;    // Small delay before opening is printed.

        openingPrinted = false;
        doClear = false;
        isOpen_dialogue = false;
        noticedAlready = false;
        task2_added = false;
        task3_added = false;

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
        tasks[1] = "Talk to the Giant Bug.";
        tasks[2] = "Use the music-maker to get past the monster.";
        tasks[3] = "Get the key and move onto the next level.";

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

    void doTaskAdd1()
    {
        closeDialogueOptions();
        
        if (!task3_added) {
            inventory.addTask(tasks[3]);
            task3_added = true;
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
        dialogueBox.text = "You: Why is it always so dark in here...";
        openingPrinted = true;
        timer = 300;
        doClear = true;
    }

    public void print_noticeBlythe()
    {
        if (!noticedAlready) {
            dialogueBox.text = "You: It's that giant bug again!";
            inventory.addTask(tasks[1]);
            timer = 300;
            doClear = true;
            noticedAlready = true;
        }
    }

    public void print_blytheTalk()
    {
        inventory.removeTask(tasks[1]);
        dialogueBox.text = "Giant Bug: Long time no see! Ready for another challenge?";
        timer = 0;
        doClear = false;
        openDialogueOptions();
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
        // Player said: "You bet! What do I have to do now?"
        if (dialogueRound == 0) {
            dialogueRound = 100;
            dialogueBox.text = "Giant Bug: Well, the key you need is right next " +
                               "to the door, so you just need to grab it and move " +
                               "onto the next floor!";
            updateDialogueBoxes(/* 100 */);
        }

        // Player said: "That seems too easy. What else should I know?"
        else if (dialogueRound == 100) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: I'm glad you asked! The monster's " +
                               "patrolling the only path to the door, so you'll " +
                               "need to use a music-maker to distract it.";
            updateDialogueBoxes(/* 102 */);
        }

        // Player said: "Right! I gotta save my friend! How do I do that?"
        else if (dialogueRound == 101) {
            dialogueRound = 201;
            dialogueBox.text = "Giant Bug: That's the spirit! The key to the " +
                               "next floor is right beside to the door! The thing " +
                               "is, the monster's there too.";
            updateDialogueBoxes(/* 201 */);
        }

        // Player said: "How is a music-maker going to help me?"
        else if (dialogueRound == 102) {
            dialogueRound = 202;
            dialogueBox.text = "Giant Bug: Well, as you know, the monster is attracted " +
                               "to sound. If you throw the music-maker with [f], the " +
                               "monster will follow it!";
            updateDialogueBoxes(/* 202 */);
        }

        // Player said: "How am I supposed to get to the door then?"
        else if (dialogueRound == 201) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: You're gonna want to distract it! See " +
                               "that little blue thing next to me? It's a music-maker!";
            updateDialogueBoxes(/* 102 */);
        }

        // Player said: "And once it's distracted, I can run past it?"
        else if (dialogueRound == 202) {
            dialogueRound = 300;
            dialogueBox.text = "Giant Bug: Yup! You'll have only a few seconds, though, " +
                               "so you'll need to be quick!";
            updateDialogueBoxes(/* 300 */);
        }

        // Player said: "Sounds good. Let's hope the music-maker works!"
        else if (dialogueRound == 300) {
            dialogueBox.text = "Giant Bug: It will! See you on the next floor!";
            doTaskAdd();
        }
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_LB()
    {
        // Player said: "Not really. I want to go home..."
        if (dialogueRound == 0) {
            dialogueRound = 101;
            dialogueBox.text = "Giant Bug: Haha, you silly goose! You've already " +
                               "gotten so far to find your friend. Turning back now ain't an option!";
            updateDialogueBoxes(/* 101 */);
        }

        // Player said: "Sounds good. Anything else I should know?"
        else if (dialogueRound == 100) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: I'm glad you asked! The monster's " +
                               "patrolling the only path to the door, so you'll " +
                               "need to use a music-maker to distract it.";
            updateDialogueBoxes(/* 102 */);
        }
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_RT() 
    {
        // Player said: "The monster. Is it on this floor too?"
        if (dialogueRound == 0) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: You betcha! Don't worry, though. I've " +
                               "gotten you a secret weapon. On the floor next to me " +
                               "is a music-maker!";
            updateDialogueBoxes(/* 102 */);
        }

        // Player said: "Will I have to deal with the monster to get there?"
        else if (dialogueRound == 100) {
            dialogueRound = 102;
            dialogueBox.text = "Yup! In fact, it's watching the only path to the door! " +
                               "Don't worry though, there's a secret weapon for you right next to me: a " +
                               "music-maker!";
            updateDialogueBoxes(/* 102 */);
        }

        // Player said: "Okay... What do I have to do?"
        else if (dialogueRound == 101) {
            dialogueRound = 100;
            dialogueBox.text = "Giant Bug: Well, the key you need is right next " +
                               "to the door, so you just need to grab it and move " +
                               "onto the next floor!";
            updateDialogueBoxes(/* 100 */);
        }

        // Player said: "A what now?"
        else if (dialogueRound == 102) {
            dialogueRound = 202;
            dialogueBox.text = "Giant Bug: A music-maker! It makes music, silly " +
                               "goose! You press [f] to throw it, and its music " +
                               "will distract the monster!";
            updateDialogueBoxes(/* 202 */);
        }

        // Player said: "So this level's impossible?"
        else if (dialogueRound == 201) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: Nope! See that blue thing next to me? It's " +
                               "a music-maker, and it's gonna help you get past the monster!";
            updateDialogueBoxes(/* 102 */);
        }

        // Player said: "Are you sure that'll work?"
        else if (dialogueRound == 202) {
            dialogueRound = 300;
            dialogueBox.text = "Giant But: Yup! It'll only be distracted for a moment, " +
                               "though, so you'll have to be quick!";
            updateDialogueBoxes(/* 300 */);
        }

        // Player said: "I didn't catch all that. Can you explain the music-maker again?"
        else if (dialogueRound == 300) {
            dialogueRound = 400;
            dialogueBox.text = "Giant Bug: Yup! You throw it by pressing [f]. The " +
                               "music-maker will then distract the monster for a moment. " +
                               "While it's distracted, get to the door.";
            updateDialogueBoxes(/* 400 */);
        }
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_RB() 
    {
        // Player said: "How did you get here so fast?"
        if (dialogueRound == 0) {
            dialogueRound = 100;
            dialogueBox.text = "Giant Bug: Don't worry about it! " +
                               "You need to get to the next level! The key is right " +
                               "beside to the door, so this should be pretty quick.";
            updateDialogueBoxes(/* 100 */);
        }

        // Player said: "Coolio! See you on the next floor!"
        else if (dialogueRound == 100) {
            dialogueBox.text = "Giant Bug: Okay! Good luck!";
            doTaskAdd1();
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
            choices[0].text = "You bet! What do I have to do now?";
            choices[1].text = "Not really. I want to go home...";
            choices[2].text = "The monster. Is it on this floor too?";
            choices[3].text = "How did you get here so fast?";
        }

        // ROUND 1.
        else if (dialogueRound == 100) {
            choices[0].text = "That seems too easy. What else should I know?";
            choices[1].text = "Sounds good. Anything else I should know?";
            choices[2].text = "Will I have to deal with the monster to get there?";
            choices[3].text = "Coolio! See you on the next floor!";
        }
        else if (dialogueRound == 101) {
            choices[0].text = "Right! I gotta save my friend! How do I do that?";
            choices[1].text = "";
            choices[2].text = "Okay... What do I have to do?";
            choices[3].text = "";
        }
        else if (dialogueRound == 102) {
            choices[0].text = "How is a music-maker going to help me?";
            choices[1].text = "";
            choices[2].text = "A what now?";
            choices[3].text = "";
        }

        // ROUND 2.
        else if (dialogueRound == 201) {
            choices[0].text = "How am I supposed to get to the door then?";
            choices[1].text = "";
            choices[2].text = "So this level's impossible?";
            choices[3].text = "";
        }
        else if (dialogueRound == 202) {
            choices[0].text = "And once it's distracted, I can run past it?";
            choices[1].text = "";
            choices[2].text = "Are you sure that'll work?";
            choices[3].text = "";
        }

        // ROUND 3.
        else if (dialogueRound == 300) {
            choices[0].text = "Sounds good. Let's hope the music-maker works!";
            choices[1].text = "";
            choices[2].text = "I didn't catch all that. Can you explain the music-maker again?";
            choices[3].text = "";
        }

        // ROUND 4.
        else if (dialogueRound == 400) {
            choices[0].text = "Got it. Let's hope the music-maker works!";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";
            dialogueRound = 300;
        }
    }
}
