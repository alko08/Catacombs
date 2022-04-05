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
        if ( (this.name == "GreenBook") && (bookOpenTrigger == false) ) {
            Debug.Log("GreenBook Open");
            GameObject.Find("MissionManager").GetComponent<mission_01>().bookTextTrigger2 = true;
            bookOpenTrigger = true;
        }
    }

    // Function that returns an item bio depending on the name of the item. As
    // more items get added, this function will get longer and messier.
    string findItemBio(string itemName)
    {
        string bio;

        if (itemName.Contains("testBook")) {
            bio = "Nothing to see here. Just a test.";
            
            return bio;
        } else if (itemName.Contains("Battery")) {
            bio = "Charges the flashlight to full when it dies.";
            
            return bio;
        } else if (itemName == "GreenBook") {
            bio = "Welcome to Tisch. If you can't remember how or when you " +
                  "got here, do not fret. The purple journal will explain " +
                  "everything.";
        } else {
            return "ITEM UNKNOWN";
        }

        return bio;
    }
}
