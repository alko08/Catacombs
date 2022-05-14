using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// For library win condition
using TMPro;
using UnityEngine.SceneManagement;

public class inventoryScript : MonoBehaviour
{
    /*********************************************************************\
        STRUCTS & VARIABLES
    \*********************************************************************/

    // Defining a struct for books.
    public struct Book
    {
        public string m_name;
        public Texture2D m_sprite;
    }
    
    // Declaring Variables.
    
    // Inventory UI vars. 
    public bool isOpen;
    
    public bool isOpen_inven;
    private GameObject inventoryUI;  // Refers to the parent containing all
                                    // inventory elements.
    public Button inventory_exit;

    // Objectives UI vars.
    public bool isOpen_tasks;
    private GameObject tasksUI;

    // Selector UI vars.
    public bool isOpen_select;
    private GameObject selectorUI, glow_o, glow_i;

    // Book bools.
    public bool firstBookFound; // Communicates with mission01 script to
                                // update dialogueBox.
    public bool firstBookRead; // Same as above.
    public bool purpleBookFound; // Same as above.
    
    // List for storing item data.
    public List<Book> inventoryList = new List<Book>();

    // List for storing item slots on UI.
    public List<GameObject> itemsUI = new List<GameObject>();
    const int numItems_UI = 32;

    // List for storing task data.
    public List<string> objectivesList = new List<string>();

    // List for storing task text on UI. 
    public List<GameObject> taskList_UI = new List<GameObject>();
    const int numTasks_UI = 6;

    // Item Textures.
    public Texture2D book0;
    public Texture2D book1;
    public Texture2D book2;
    public Texture2D book3;
    public Texture2D keyRing;
    public Texture2D batteyTexture;
    public Texture2D speakerTexture;

    // Batteries, Speakers, and Keys
    private int batteryCount, speakerCount;
    public bool hasKey;
    private TextMeshProUGUI batteryCountText, speakerCountText;

    // Keeping track of books.
    public TextMeshProUGUI points;
    public int numBooks;

    // Other.
    private int repeats;
    public TextMeshProUGUI goalTextMeshPro;
    public bool dialogue_open;
    public int currMission;
    private GameObject sprintBarObject;
    private bool hasBattery, hasSpeaker;

    /*********************************************************************\
        FUNCTIONS
    \*********************************************************************/
    
    // Begin by hiding all inventory UI elements.
    void Start()
    {
        // Initializing Key.
        hasKey = false;

        // Initializing Batteries.
        batteryCount = StaticVariables.batteryNum;
        batteryCountText = GameObject.FindWithTag("BatteryCount").GetComponent<TextMeshProUGUI>();
        batteryCountText.SetText("" + batteryCount);

        // Initializing Speakers.
        speakerCount = StaticVariables.speakerNum;
        speakerCountText = GameObject.FindWithTag("SpeakerCount").GetComponent<TextMeshProUGUI>();
        speakerCountText.SetText("" + speakerCount);

        // Initializing inventoryUI.
        isOpen_inven = false;
        inventoryUI = GameObject.Find("inventoryUI");
        inventory_exit = GameObject.Find("inventory_exit").GetComponent<Button>();
        inventory_exit.onClick.AddListener(InventoryExit_Clicked);
        initiateItemsUI();

        // Initializing objectives menu.
        isOpen_tasks = false;
        tasksUI = GameObject.Find("objectivesUI");
        inventory_exit = GameObject.Find("objective_exit").GetComponent<Button>();
        inventory_exit.onClick.AddListener(ObjectiveExit_Clicked);
        initiateTasksUI();

        // Initializing selector.
        isOpen_select = false;
        selectorUI = GameObject.Find("selectorUI");
        glow_o = selectorUI.transform.GetChild(0).gameObject;
        glow_i = selectorUI.transform.GetChild(1).gameObject;

        // Setting objective bools.
        firstBookFound  = false;
        firstBookRead   = false;
        purpleBookFound = false;

        // Closing UI so player starts with it closed.
        inventoryUI.SetActive(false);
        tasksUI.SetActive(false);
        selectorUI.SetActive(false);

        // Book tracking.
        if (currMission == 1) {
            points = GameObject.Find("Points").GetComponent<TextMeshProUGUI>();
        }
        numBooks = 0;

        // Other.
        dialogue_open = false;
        isOpen = false;
        hasBattery = false;
        hasSpeaker = false;
        sprintBarObject = GameObject.FindWithTag("SprintCharge");

        if (batteryCount > 0) {
            inventoryList.Add(new Book() 
            { m_name = "A Battery", 
                m_sprite = batteyTexture });
            hasBattery = true;
        }
        if (speakerCount > 0) {
            inventoryList.Add(new Book() 
            { m_name = "A Music-Maker", 
                m_sprite = speakerTexture });
            hasSpeaker = true;
        }
    }

    // This function should grab all the item GameObjects in the UI and store
    // them in a single list that we can reference later. Afterwards, it hides
    // all the GameObjects because the inventory starts off empty.
    void initiateItemsUI()
    {
        string currItemName;
        
        for (int i = 0; i < numItems_UI; i++) {
            currItemName = "item" + i.ToString();
            // Debug.Log("Processing " + currItemName);
            itemsUI.Add(GameObject.Find(currItemName));
            itemsUI[i].SetActive(false);
        }
    }

    // This function grabs all the task GameObjects in objectivesUI and stores
    // them in a list that we can reference later. 
    void initiateTasksUI()
    {
        string currTask;

        for (int i = 0; i < numTasks_UI + 1; i++) {
            currTask = "task" + i.ToString();
            // Debug.Log("Processing " + currItemName);
            taskList_UI.Add(GameObject.Find(currTask));
            taskList_UI[i].SetActive(false);
        }
    }

    // On update, check if the [R] key was pressed. If it was, switch the
    // of the inventory. Don't open if dialogue is open.
    void Update()
    {
        isOpen = isOpen_select || isOpen_inven || isOpen_tasks;

        dialogue_open = GameObject.Find("missionManager").GetComponent<mission_00>().isOpen_dialogue
        || GameObject.Find("missionManager").GetComponent<mission_01>().isOpen_dialogue
        || GameObject.Find("missionManager").GetComponent<mission_02>().isOpen_dialogue
        || GameObject.Find("missionManager").GetComponent<mission_03>().isOpen_dialogue;
        
        if ((Input.GetButtonDown("Inventory")) && (!dialogue_open)) {
            // Code that opens the selector. 
            if ( (!isOpen_select) && (!isOpen_inven) && (!isOpen_tasks) ){
                doOpen_select();
            } else if (isOpen_inven) {
                doClose();
            } else if (isOpen_tasks) {
                doClose_tasks();
            } else {
                doClose_select();
            }
        }
    }

    // OPEN/CLOSE FUNCTIONS
    
    // inventoryUI
    void doOpen()
    {
        // Opening UI elements.
        inventoryUI.SetActive(true);
        for (int i = 0; i < inventoryList.Count; i++) {
            displayItem(inventoryList[i], i);
        }

        // Disabling out-of-UI controls and unlocking cursor. 
        // GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = true;
        setNonUI(false);

        isOpen_inven = !isOpen_inven;
    }

    void doClose()
    {
        inventoryUI.SetActive(false);

        setNonUI(true);

        isOpen_inven = !isOpen_inven;
    }

    // tasksUI
    void doOpen_tasks()
    {
        tasksUI.SetActive(true);
        for (int i = 0; i < objectivesList.Count; i++) {
            printTask(objectivesList[i], i);
        }

        setNonUI(false);

        isOpen_tasks = !isOpen_tasks;
    }

    void doClose_tasks()
    {
        tasksUI.SetActive(false);

        setNonUI(true);

        isOpen_tasks = !isOpen_tasks;
    }

    // selectorUI
    void doOpen_select()
    {
        selectorUI.SetActive(true);

        setNonUI(false);

        isOpen_select = !isOpen_select;
    }

    void doClose_select()
    {
        glow_o.SetActive(false);
        glow_i.SetActive(false);
        GameObject.Find("ObjectivesButton").GetComponent<SelectorGlowScript>().mousingOver = false;
        GameObject.Find("InventoryButton").GetComponent<SelectorGlowScript>().mousingOver = false;
        selectorUI.SetActive(false);

        setNonUI(true);

        isOpen_select = !isOpen_select;

        if (isOpen_inven) {
            doClose();
        }
    }

    // Helper to toggling out-of-UI controls. The boolean parameter represents the status of non-UI elements.
    // Pass false for opening a UI element, true when closing it. 
    void setNonUI(bool NonUI_status)
    {
        
        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = NonUI_status;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = !NonUI_status;

        if (!NonUI_status) {
            Time.timeScale = 0f;
            sprintBarObject.SetActive(false);
        } else {
            Time.timeScale = 1f;
        }
    }

    // Function for adding books to inventory.
    public void addBook(string bookName)
    {
        if (bookName.Contains("pickup_green")) {
            inventoryList.Add(new Book() 
                { m_name = "Welcome", 
                  m_sprite = book1 });
            
            if (firstBookFound == false) {
                firstBookFound = true;
            }
        } 
        
        // For non-books, we want to cancel out the numBooks++ down below.
        else if (bookName.Contains("battery")) {
            batteryCount++;
            batteryCountText.SetText("" + batteryCount);
            if (!hasBattery) {
                inventoryList.Add(new Book() 
                { m_name = "A Battery", 
                  m_sprite = batteyTexture });
                hasBattery = true;
            }
            numBooks--;
        } else if (bookName.Contains("speaker")) {
            speakerCount++;
            speakerCountText.SetText("" + speakerCount);
            if (!hasSpeaker) {
                inventoryList.Add(new Book() 
                { m_name = "A Music-Maker", 
                  m_sprite = speakerTexture });
                hasSpeaker = true;
            }
            numBooks--;
        } else if (bookName.Contains("keyring")) {
            hasKey = true;
            inventoryList.Add(new Book() 
                { m_name = "A Ring of Keys", 
                  m_sprite = keyRing });
            numBooks--;
        } 
        
        else if (bookName.Contains("pickup_hint")) {
            inventoryList.Add(new Book()
                { m_name = "A Hint",
                  m_sprite = book0});
        } else if (bookName.Contains("pickup_purple")) {
            inventoryList.Add(new Book()
                { m_name = "Journal",
                  m_sprite = book2});
            
            if (purpleBookFound == false) {
                purpleBookFound = true;
            }
        
        } else if (bookName.Contains("test_pickup")) {
            inventoryList.Add(new Book() 
                { m_name = randomBookName(-1), 
                  m_sprite = book3 });
        } else if (bookName.Contains("turn back")) {
            inventoryList.Add(new Book()
                { m_name = "Turn Back",
                  m_sprite = book2 });
        } else if (bookName.Contains("midas project 1")) {
            inventoryList.Add(new Book()
                { m_name = "Midas Project 1",
                  m_sprite = book1 });
        } else if (bookName.Contains("midas project 2")) {
            inventoryList.Add(new Book()
                { m_name = "Midas Project 2",
                  m_sprite = book1 });
        } else if (bookName.Contains("hint2")) {
            inventoryList.Add(new Book()
                { m_name = "2nd Hint",
                  m_sprite = book1 });
        } else if (bookName.Contains("final warning")) {
            inventoryList.Add(new Book()
                { m_name = "A Warning",
                  m_sprite = book2 });
        }

        else if (bookName.Contains("tutorialBook1")) {
            inventoryList.Add(new Book()
                { m_name = "It Listens",
                  m_sprite = book1});
            addTask("Find the key and go deeper into Tisch.");
        } else if (bookName.Contains("tutorialBook2")) {
            inventoryList.Add(new Book()
                { m_name = "Tread Softly",
                  m_sprite = book1});
        } else if (bookName.Contains("tutorialBook3")) {
            inventoryList.Add(new Book()
                { m_name = "Stay Hidden",
                  m_sprite = book1});
        }

        else if (bookName.Contains("tutorialBook4")) {
            inventoryList.Add(new Book()
                { m_name = "Loud Sounds",
                  m_sprite = book1});
        } else if (bookName.Contains("randomBook2")) {
            inventoryList.Add(new Book()
                { m_name = randomBookName2(-1),
                  m_sprite = book3});
        }

        else if (bookName.Contains("tutorialBook5")) {
            inventoryList.Add(new Book()
                { m_name = "The Maze",
                  m_sprite = book1});
            removeTask("Read the book beside the table.");
            addTask("Find the key at the center of the maze.");
        } else if (bookName.Contains("randomBook3")) {
            inventoryList.Add(new Book()
                { m_name = randomBookName3(-1),
                  m_sprite = book3});
        }

        else if (bookName.Contains("tutorialBook6")) {
            inventoryList.Add(new Book()
                { m_name = "The Catacombs",
                  m_sprite = book1});
        } else if (bookName.Contains("randomBook4")) {
            inventoryList.Add(new Book()
                { m_name = randomBookName4(-1),
                  m_sprite = book3});
        }

        if (isOpen_inven) {
            doOpen();
        }

        if (currMission == 1) {
            numBooks++;
            points.text = numBooks.ToString() + " / 10";
        }
    }

    void displayItem(Book book, int index)
    {
        if (index < itemsUI.Count) {
            GameObject currItem = itemsUI[index];

            currItem.SetActive(true);
            currItem.name = book.m_name;
            currItem.GetComponent<RawImage>().texture = book.m_sprite;
        } else {
            Debug.Log("index out of range.");
        }
    }

    public void addTask(string newTask)
    {
        objectivesList.Add(newTask);

        updateTasks();
    }

    public void removeTask(string removedTask) 
    {
        int countAtStart = objectivesList.Count;
        
        Debug.Log("Removing task: " + removedTask);
        
        objectivesList.Remove(removedTask);
        
        if (objectivesList.Count < countAtStart) {
            Debug.Log("Task removed.");
        } else {
            Debug.Log("Task not removed.");
        }

        updateTasks();
    }

    void updateTasks()
    {
        if (isOpen_tasks) {
            for (int i = 0; i < objectivesList.Count; i++) {
                printTask(objectivesList[i], i);
            }
        }
    }

    void printTask(string task, int index)
    {
        if (index < taskList_UI.Count) {
            taskList_UI[index].SetActive(true);
            taskList_UI[index].GetComponent<TextMeshProUGUI>().text = task;
        } else {
            Debug.Log("index out of range.");
        }
    }

    public void removeBook(string bookName)
    {
        if (bookName == "keyRing") {
            inventoryList.Remove(new Book()
                { m_name = "A Ring of Keys", 
                  m_sprite = keyRing });
            itemsUI[inventoryList.Count].SetActive(false);
            hasKey = false;
            // Debug.Log("Cant remove:" + bookName);
        } 

        else if (bookName == "ALL_OF_THEM") {
            inventoryList.Clear();
            if (batteryCount > 0) {
                inventoryList.Add(new Book() 
                { m_name = "A Battery", 
                  m_sprite = batteyTexture });
                hasBattery = true;
            }
            if (speakerCount > 0) {
                inventoryList.Add(new Book() 
                { m_name = "A Music-Maker", 
                  m_sprite = speakerTexture });
                hasSpeaker = true;
            }
        }
        
        
        else {
            Debug.Log("Cant remove:" + bookName);
        }
        
        if (isOpen_inven) {
            doOpen();
        }
    }

    public bool containsBattery() {
        return batteryCount > 0;
    }

    public void removeBattery()
    {
        if (batteryCount > 0) {
            batteryCount--;
            batteryCountText.SetText("" + batteryCount);
        }

        if (batteryCount == 0 && hasBattery) {
            inventoryList.Remove(new Book()
                { m_name = "A Battery", 
                  m_sprite = batteyTexture });
            itemsUI[inventoryList.Count].SetActive(false);
            hasBattery = false;
            if (isOpen_inven) {
                doOpen();
            }
        }
    }

    public bool containsSpeaker() {
        return speakerCount > 0;
    }

    public void removeSpeaker()
    {
        if (speakerCount > 0) {
            speakerCount--;
            speakerCountText.SetText("" + speakerCount);
        }

        if (speakerCount == 0 && hasSpeaker) {
            inventoryList.Remove(new Book()
                { m_name = "A Music-Maker", 
                m_sprite = speakerTexture });
            itemsUI[inventoryList.Count].SetActive(false);
            hasSpeaker = false;
            if (isOpen_inven) {
                doOpen();
            }
        }
    }

    string randomBookName(int index)
    {
        string text = "";
        if (index == -1) {
            repeats = 0;
            System.Random rnd = new System.Random();
            index = rnd.Next(10);
        } else {
            repeats++;
            index++;
            if (index > 9) {
                index = 0;
            }
        }

        if (index == 0) {
            text = "Rats";
        }

        else if (index == 1) {
            text = "A Poem";
        }

        else if (index == 2) {
            text = "The Wretch";
        }

        else if (index == 3) {
            text = "Notes 1";
        }

        else if (index == 4) {
            text = "Notes 2";
        }

        else if (index == 5) {
            text = "The Meldon Archives: Vol 1";
        }

        else if (index == 6) {
            text = "The Final Hours";
        }

        else if (index == 7) {
            text = "Pebble Ghosts";
        }

        else if (index == 8) {
            text = "Blythe The Bug";
        }

        else if (index == 9) {
            text = "The Final Hours 2";
        }

        Debug.Log("Index:" + index);
        bool exists = false;
        for (int i = 0; i < inventoryList.Count; i++) {
            if (text == inventoryList[i].m_name && repeats < 10) {
                exists = true;
            }
        }

        if (exists) {
            return randomBookName(index);
        } else {
            return text;
        }
    }

    string randomBookName2(int index)
    {
        string text = "";
        if (index == -1) {
            repeats = 0;
            System.Random rnd = new System.Random();
            index = rnd.Next(5);
        } else {
            repeats++;
            index++;
            if (index > 4) {
                index = 0;
            }
        }

        if (index == 0) {
            text = "Pots & Sculptures";
        }

        else if (index == 1) {
            text = "Pots & Sculptures 2";
        }

        else if (index == 2) {
            text = "Milod Quotes 1";
        }

        else if (index == 3) {
            text = "It Is Hunting You";
        }

        else if (index == 4) {
            text = "Technology & Magic";
        }

        Debug.Log("Index:" + index);
        bool exists = false;
        for (int i = 0; i < inventoryList.Count; i++) {
            if (text == inventoryList[i].m_name && repeats < 5) {
                exists = true;
            }
        }

        if (exists) {
            return randomBookName2(index);
        } else {
            return text;
        }
    }

    string randomBookName3(int index)
    {
        string text = "";
        if (index == -1) {
            repeats = 0;
            System.Random rnd = new System.Random();
            index = rnd.Next(6);
        } else {
            repeats++;
            index++;
            if (index > 5) {
                index = 0;
            }
        }

        if (index == 0) {
            text = "IT FOUND ME";
        }

        else if (index == 1) {
            text = "HELP ME";
        }

        else if (index == 2) {
            text = "Dogs and Cats";
        }

        else if (index == 3) {
            text = "Among Us Car";
        }

        else if (index == 4) {
            text = "Computer Science";
        }

        else if (index == 5) {
            text = "Lab Assitants";
        }

        Debug.Log("Index:" + index);
        bool exists = false;
        for (int i = 0; i < inventoryList.Count; i++) {
            if (text == inventoryList[i].m_name && repeats < 6) {
                exists = true;
            }
        }

        if (exists) {
            return randomBookName3(index);
        } else {
            return text;
        }
    }

    string randomBookName4(int index)
    {
        string text = "";
        if (index == -1) {
            repeats = 0;
            System.Random rnd = new System.Random();
            index = rnd.Next(6);
        } else {
            repeats++;
            index++;
            if (index > 5) {
                index = 0;
            }
        }

        if (index == 0) {
            text = "Chronicles of Big Sash 1";
        }

        else if (index == 1) {
            text = "Chronicles of Big Sash 2";
        }

        else if (index == 2) {
            text = "Chronicles of Big Sash 3";
        }

        else if (index == 3) {
            text = "Chronicles of Big Sash 4";
        }

        else if (index == 4) {
            text = "Chronicles of Big Sash 5";
        }

        else if (index == 5) {
            text = "Who Wrote These Books?";
        }

        Debug.Log("Index:" + index);
        bool exists = false;
        for (int i = 0; i < inventoryList.Count; i++) {
            if (text == inventoryList[i].m_name && repeats < 6) {
                exists = true;
            }
        }

        if (exists) {
            return randomBookName4(index);
        } else {
            return text;
        }
    }

    // Wrapper functions used by SelectorGlowScript to quickly switch from
    // the selector screen to one of the other UI screens.
    public void SelectToInventory()
    {
        doClose_select();
        doOpen();
    }

    public void SelectToTasks()
    {
        doClose_select();
        doOpen_tasks();
    }

    public void updateVariables() {
        StaticVariables.batteryNum = batteryCount;
        StaticVariables.speakerNum = speakerCount;
    }

    // Function for the inventory exit button
    void InventoryExit_Clicked()
    {
        doClose();
    }

    // Function for the objective exit button
    void ObjectiveExit_Clicked()
    {
        doClose_tasks();
    }
}
