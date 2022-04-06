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

    // Item Textures.
    public Texture2D book0;
    public Texture2D book1;
    public Texture2D book2;
    public Texture2D book3;
    public Texture2D battery;

    // Other.
    private int testTotal;
    public TextMeshProUGUI goalTextMeshPro;

    /*********************************************************************\
        FUNCTIONS
    \*********************************************************************/
    
    // Begin by hiding all inventory UI elements.
    void Start()
    {
        isOpen = false;
        inventoryUI = GameObject.Find("inventoryUI");
        initiateItemsUI();

        isOpen_select = false;
        selectorUI = GameObject.Find("selectorUI");
        selectorUI.SetActive(false);

        firstBookFound  = false;
        firstBookRead   = false;
        purpleBookFound = false;

        inventoryUI.SetActive(false);
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

    // On update, check if the [R] key was pressed. If it was, switch the
    // of the inventory.
    void Update()
    {
        if (Input.GetButtonDown("Inventory")) {
            if (!isOpen) {
                doOpen();
            } else {
                doClose();
            }

            isOpen = !isOpen;

            // if (!isOpen_select) {
            //     doOpen_select();
            // } else {
            //     doClose_select();
            // }

            // isOpen = !isOpen;
        }
    }

    void doOpen()
    {
        // Opening UI elements.
        inventoryUI.SetActive(true);
        Screen.lockCursor = false;
        for (int i = 0; i < inventoryList.Count; i++) {
            displayItem(inventoryList[i], i);
        }

        // Disabling out-of-UI controls and unlocking cursor. 
        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void doOpen_select()
    {
        selectorUI.SetActive(true);
        Screen.lockCursor = false;

        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void doClose_select()
    {
        selectorUI.SetActive(false);
        Screen.lockCursor = true;
        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
    }

    void doClose()
    {
        inventoryUI.SetActive(false);
        Screen.lockCursor = true;
        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
    }

    // Function for adding books to inventory.
    public void addBook(string bookName)
    {
        if (bookName.Contains("pickup_green")) {
            inventoryList.Add(new Book() 
                { m_name = "Green Book", 
                  m_sprite = book1 });
            
            if (firstBookFound == false) {
                firstBookFound = true;
            }
        } else if (bookName.Contains("battery")) {
            inventoryList.Add(new Book() 
                { m_name = "Battery", 
                  m_sprite = battery });
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

    public void removeBook(string bookName)
    {
        if (bookName == "Battery") {
            inventoryList.Remove(new Book()
                { m_name = "Battery", 
                  m_sprite = battery });
            itemsUI[inventoryList.Count].SetActive(false);
        } else {
            Debug.Log("Cant remove:" + bookName);
        }
        
        if (isOpen) {
            doOpen();
        }
    }

    public bool containsBattery() {
        return inventoryList.Contains(new Book()
                { m_name = "Battery", 
                  m_sprite = battery });
    }

    string randomBookName()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(3);
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

        return text;
    }
}
