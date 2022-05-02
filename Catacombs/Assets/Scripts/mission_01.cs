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

        initiateBoxes();
        dialogueRound = 0;
        GameObject.Find("blythe").GetComponent<blythe>().currMission = 1;

        timer = 120;    // Small delay before opening is printed.

        openingPrinted = false;
        doClear = false;
        isOpen_dialogue = false;

        initiateTasks();
    }

    // Update is called once per frame. The way we do dialogue is by having
    // a timer that constantly ticks down. Each time it hits 0, this function
    // will do something that affects the dialogue box.
    void Update()
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
        tasks[1] = "Find and speak to the Giant Bug.";

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
        timer = 300;
        doClear = true;
    }

    public void print_noticeBlythe()
    {
        dialogueBox.text = "You: Is that the Giant Bug?";
        inventory.addTask(tasks[1]);
        timer = 300;
        doClear = true;
    }

    public void print_blytheTalk()
    {
        inventory.removeTask(tasks[1]);
        dialogueBox.text = "Giant Bug: Hey, it's you again! I hope the monster " +
                           "wasn't too bad!";
        timer = 0;
        doClear = false;
        openDialogueOptions();
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
        if (dialogueRound == 0) {
            dialogueRound = 100;
            dialogueBox.text = "Giant Bug: ";
            updateDialogueBoxes(/* 100 */);
        } 
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_LB()
    {
        if (dialogueRound == 0) {
            dialogueRound = 101;
            dialogueBox.text = "Giant Bug: ";
            updateDialogueBoxes(/* 101 */);
        } 
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_RT() 
    {
        if (dialogueRound == 0) {
            dialogueRound = 102;
            dialogueBox.text = "Giant Bug: ";
            updateDialogueBoxes(/* 102 */);
        } 
        
        else {
            closeDialogueOptions();
        }
    }

    void ButtonClicked_RB() 
    {
        if (dialogueRound == 0) {
            dialogueRound = 103;
            dialogueBox.text = "Giant Bug: ";
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
            choices[0].text = "";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";
        }
        else if (dialogueRound == 101) {
            choices[0].text = "";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";
        }
        else if (dialogueRound == 102) {
            choices[0].text = "";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";
        }
        else if (dialogueRound == 103) {
            choices[0].text = "";
            choices[1].text = "";
            choices[2].text = "";
            choices[3].text = "";
        }
    }
}
