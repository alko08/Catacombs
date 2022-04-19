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

    public GameObject inventoryUI;  // Refers to the parent containing all
                                    // inventory elements.

    // Objectives UI vars.
    public bool isOpen_tasks;
    public GameObject tasksUI;

    // Selector UI vars.
    public bool isOpen_select;
    public GameObject selectorUI;

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
    // public Texture2D battery;
    public int batteryCount;
    public bool hasKey;
    private TextMeshProUGUI batteryCountText;

    // Other.
    private int testTotal;
    public TextMeshProUGUI goalTextMeshPro;

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

        // Initializing inventoryUI.
        isOpen = false;
        inventoryUI = GameObject.Find("inventoryUI");
        initiateItemsUI();

        // Initializing objectives menu.
        isOpen_tasks = false;
        tasksUI = GameObject.Find("objectivesUI");
        initiateTasksUI();

        // Initializing selector.
        isOpen_select = false;
        selectorUI = GameObject.Find("selectorUI");

        // Setting objective bools.
        firstBookFound  = false;
        firstBookRead   = false;
        purpleBookFound = false;

        // Closing UI so player starts with it closed.
        inventoryUI.SetActive(false);
        tasksUI.SetActive(false);
        selectorUI.SetActive(false);

        // Other.
        testTotal = 0;
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
    // of the inventory.
    void Update()
    {
        if (Input.GetButtonDown("Inventory")) {
            // Code that opens the selector. 
            if ( (!isOpen_select) && (!isOpen) && (!isOpen_tasks) ){
                doOpen_select();
            } else if (isOpen) {
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

        isOpen = !isOpen;
    }

    void doClose()
    {
        inventoryUI.SetActive(false);

        setNonUI(true);

        isOpen = !isOpen;
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
        selectorUI.SetActive(false);

        setNonUI(true);

        isOpen_select = !isOpen_select;

        if (isOpen) {
            doClose();
        }
    }

    // Helper to toggling out-of-UI controls. The boolean parameter represents the status of non-UI elements.
    // Pass false for opening a UI element, true for closing it. 
    void setNonUI(bool NonUI_status)
    {
        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = NonUI_status;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = !NonUI_status;
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
        } else if (bookName.Contains("battery")) {
            batteryCount++;
            batteryCountText.SetText("" + batteryCount);
        } else if (bookName.Contains("keyring")) {
            hasKey = true;
            inventoryList.Add(new Book() 
                { m_name = "A Ring of Keys", 
                  m_sprite = keyRing });
        } else if (bookName.Contains("pickup_hint")) {
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
                { m_name = randomBookName(), 
                  m_sprite = book3 });
            testTotal++;
            goalTextMeshPro.SetText(testTotal + " / 10");
            if (testTotal >= 10) {
                SceneManager.LoadScene("WinScene");
            }
        
        } else if (bookName.Contains("turn back")) {
            inventoryList.Add(new Book()
                { m_name = "Turn Back",
                  m_sprite = book1 });
        } else if (bookName.Contains("midas project 1")) {
            inventoryList.Add(new Book()
                { m_name = "Midas Project 1",
                  m_sprite = book0 });
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
                  m_sprite = book1 });
        }

        if (isOpen) {
            doOpen();
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

    void printTask(string task, int index)
    {
        GameObject currTask;
        if (index < taskList_UI.Count) {
            currTask = taskList_UI[index];

            currTask.SetActive(true);
            currTask.GetComponent<TextMeshProUGUI>().text = task;
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
        } else {
            Debug.Log("Cant remove:" + bookName);
        }
        
        if (isOpen) {
            doOpen();
        }
    }

    public bool containsBattery() {
        // return inventoryList.Contains(new Book()
        //         { m_name = "Battery", 
        //           m_sprite = battery });

        return batteryCount > 0;
    }

    public void removeBattery()
    {
        if (batteryCount > 0) {
            batteryCount--;
            batteryCountText.SetText("" + batteryCount);
        }
    }

    string randomBookName()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(6);
        string text = "";

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

        return text;
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
    }
}
