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
    public GameObject gameUI;
    public mission_01 nextMission;
    public TextMeshProUGUI dialogueBox;

    // Dialogue Choices
    const int NUM_BOXES = 4;
    public GameObject[] boxes;
    public TextMeshProUGUI[] choices;
    public Button[] buttons;
    
    // Timers
    private int timer1;

    // Bools
    bool opening_done;
    bool blytheTalk_done;
    bool notClear;
    bool pause;
    bool isOpen_dialogue;
    
    /*********************************************************************\
        Basic Functions
    \*********************************************************************/

    // Initializing.
    void Start()
    {
        // UI GameObject.
        gameUI = GameObject.Find("prefab_UI");

        // Other GameObjects.
        nextMission = gameObject.GetComponent<mission_01>();
            nextMission.enabled = false;
        dialogueBox = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            clear();
        
        // Dialogue Choices.
        initiateBoxes();
        
        // Timers.
        timer1 = 120;

        // Bools.
        opening_done = false;
        blytheTalk_done = false;
        pause = false;
        isOpen_dialogue = false;
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
        } else if (blytheTalk_done) {
            print_blytheTalk2();
        }
    }

    /*********************************************************************\
        Helper Functions
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
            buttons[i].onClick.AddListener(() => ButtonClicked(i));

            boxes[i].SetActive(false);
        }
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
        timer1 = 300;
        opening_done = true;
        notClear = true;
    }

    public void print_blytheTalk1()
    {
        dialogueBox.text = "Giant Bug: Oh hey there! How're you doing?";
        timer1 = 0;
        pause = true;   // Pausing text change to open talking menu.
        openDialogueOptions();
    }

    void print_blytheTalk2()
    {
        dialogueBox.text = "Giant Bug: Look, if you're confused, I'd suggest lookin " +
                           "around. In case ya couldn't tell, we're in Tisch " +
                           "library right now.";
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

        choices[0].text = "Great! How about you?";
        choices[1].text = "Where am I? Who are you? How are you talking?";
        choices[2].text = "Am I dead?";
        choices[3].text = "AAHHHHH! GIANT BUG! AAHHHHH!";
    }

    void closeDialogueOptions()
    {
        setNonUI(true);
        isOpen_dialogue = false;

        for (int i = 0; i < NUM_BOXES; i++) {
            boxes[i].SetActive(false);
        }
    }

    void ButtonClicked(int buttonNo)
    {
        Debug.Log("Button pressed: " + buttonNo);
        
        // if (buttonNo == 0) {
        //     dialogueBox.text = "Giant Bug: I'm doing great too!";
        // } else if (buttonNo == 1) {
        //     dialogueBox.text = "Giant Bug: You're asking a lotta questions, friend!";
        // } else if (buttonNo == 2) {
        //     dialogueBox.text = "Giant Bug: Nope!";
        // } else if (buttonNo == 3) {
        //     dialogueBox.text = "Giant Bug: Hey, hey! Calm down! I don't bite!";
        // }

        timer1 = 450;   // Should give a moment before the next dialogue thing appears.
        blytheTalk_done = true;
        closeDialogueOptions();
    }

    // Toggling non-UI controls. Yoinked straight from inventoryScript.
    void setNonUI(bool NonUI_status)
    {
        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = NonUI_status;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = !NonUI_status;
    }

}
