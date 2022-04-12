using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class itemDetailsScript : MonoBehaviour
{
    public inventoryScript inventory;
    public Button button;

    // Components of Details Screen.
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemBio;
    public GameObject itemImage;
    public bool bookOpenTrigger;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        itemName = GameObject.Find("details_name").GetComponent<TextMeshProUGUI>();
        itemBio = GameObject.Find("details_bio").GetComponent<TextMeshProUGUI>();
        itemImage = GameObject.Find("details_image");

        bookOpenTrigger = false;
    }

    // Function to change the details screen.
    void TaskOnClick()
    {
        Debug.Log("Button Pressed");
        
        itemName.text = this.name;
        itemImage.GetComponent<RawImage>().texture 
            = gameObject.GetComponent<RawImage>().texture;

        itemBio.text = findItemBio(this.name);

        // Update bookOpenTrigger.
        if ( (this.name == "Green Book") && (bookOpenTrigger == false) ) {
            activateBookTrigger();
        }
    }

    // Function that returns an item bio depending on the name of the item. As
    // more items get added, this function will get longer and messier.
    string findItemBio(string itemName)
    {
        string bio;

        if (itemName.Contains("testBook")) {
            bio = "Nothing to see here. Just a test.";
        } 
        
        else if (itemName.Contains("Battery")) {
            bio = "Charges the flashlight to full when it dies.";
        } 
        
        else if (itemName.Contains("Green Book")) {
            bio = "Welcome to Tisch. If you can't remember how or when you " +
                  "got here, don't worry. The purple journal will explain " +
                  "everything.";
        } 
        
        else if (itemName.Contains("A Hint")) {
            bio = "If journal is what you seek, search the opposite corner" +
                  "of the library.";
        } 

        else if (itemName.Contains("Journal")) {
            bio = "We had no choice but to wipe your memory. You simply" +
                  "learned too much. We suggest that you leave before you" +
                  "uncover our secrets again.";
        }

        else if (itemName.Contains("Rats")) {
            bio = "There are rats in the walls. There are rats in the walls. " +
                  "There are rats in the walls. There are rats in the walls.";
        }

        else if (itemName.Contains("A Poem")) {
            bio = "In the dark, I seem alone.\n" + "But there's " +
                  "a monster, my light has shown.";
        }

        else if (itemName.Contains("The Wretch")) {
            bio = "There is a monster that lurks between the shelves. It " +
                  "is called the wretch. It hungers for human flesh, and " +
                  "fears light";
        }

        else if (itemName.Contains("Notes 1")) {
            bio = "I believe that there's something sinister happening " +
                  "under Tisch. Students have gone missing, and monsters have" +
                  "begun lurking the halls.";
        }

        else if (itemName.Contains("Notes 2")) {
            bio = "I have come down here to investigate those disappearances, " +
                  "and get to the bottom of what's behind the monsters. Wish " +
                  "me luck.";
        }

        else if (itemName.Contains("The Meldon Archives: Vol 1")) {
            bio = "\n \n \n \n Shark Meldon Returns";
        }
        
        else {
            bio = "ITEM UNKNOWN";
        }

        return bio;
    }

    // Function that lets mission_01 know when to print the 2nd dialogue text.
    void activateBookTrigger()
    {
        Debug.Log("GreenBook Open");
        GameObject.Find("missionManager").GetComponent<mission_01>().bookTextTrigger2 = true;
        bookOpenTrigger = true;
    }
}
