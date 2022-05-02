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
            bio = "Charges the flashlight an extra 50%.";
        } 

        else if (itemName.Contains("A Ring of Keys")) {
            bio = "A ring of keys. Unlocks locked doors.";
        } 
        
        else if (itemName.Contains("Welcome")) {
            bio = "I see you got past the monster. Congratulations. I'm sorry " +
                  "to say that this is the furthest you will get. Turn around " +
                  "now or face the consequences.";
        } 
        
        else if (itemName.Contains("A Hint")) {
            bio = "If journal is what you seek, search the opposite corner " +
                  "of the library.";
        } 

        else if (itemName.Contains("Journal")) {
            bio = "We had no choice but to wipe your memory. You simply " +
                  "learned too much. We suggest that you leave before you " +
                  "uncover our secrets again.";
        }

        else if (itemName.Contains("Rats")) {
            bio = "There are rats in the walls. There are rats in the walls. " +
                  "There are rats in the walls. There are rats in the walls. ";
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
                  "under Tisch. Students have gone missing, and monsters have " +
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

        else if (itemName.Contains("Turn Back")) {
            bio = "In case our warning was insufficient, know that we've " +
                  "placed a guardian on this level. Hopefully it will keep " +
                  "you at bay.";
        }

        else if (itemName.Contains("Midas Project 1")) {
            bio = "April 12, 2018. After using over $2 million of student " +
                  "tuition money, we've finally managed to distill pure " +
                  "Monaconium. The future of the project looks bright.";
        }

        else if (itemName.Contains("Midas Project 2")) {
            bio = "However, several students have begun wondering what their " +
                  "tuition is being used for here. Some have gotten dangerously" +
                  "close to finding the truth...";
        }

        else if (itemName.Contains("2nd Hint")) {
            bio = "Around a corner and a turn.\n" + 
                  "More secrets you will learn.";
        }

        else if (itemName.Contains("A Warning")) {
            bio = "I thought we asked you to leave. Very well. If you intend " +
                  "on continuing your pointless search for answers, know that " +
                  "there are more monsters ahead.";
        }

        else if (itemName.Contains("It Listens")) {
            bio = "You are safe outside the glass. Watch the monster's movements. " +
                  "Predict its movements. Hide under tables. Get the keys. Progress.";
        }

        else if (itemName.Contains("Tread Softly")) {
            bio = "Sprinting draws the monster's attention. Do so only when it " +
                  "isn't nearby, or when absolutely necessary.";
        }

        else if (itemName.Contains("Stay Hidden")) {
            bio = "Hide under desks to avoid its sight.";
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
        // GameObject.Find("missionManager").GetComponent<mission_01>().bookTextTrigger2 = true;
        bookOpenTrigger = true;
    }
}
