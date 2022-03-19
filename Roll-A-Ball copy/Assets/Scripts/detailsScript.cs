using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class detailsScript : MonoBehaviour
{
    public inventoryScript inventory;
    public Button button;

    // Components of Details Screen.
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemBio;
    public GameObject itemImage;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<inventoryScript>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        itemName = GameObject.Find("itemName").GetComponent<TextMeshProUGUI>();
        itemBio = GameObject.Find("itemBio").GetComponent<TextMeshProUGUI>();
        itemImage = GameObject.Find("itemSprite");
    }

    // Function to change the text on the details screen.
    void TaskOnClick()
    {
        Debug.Log("Button Pressed");
        
        itemName.text = this.name;
        itemImage.GetComponent<RawImage>().texture 
            = gameObject.GetComponent<RawImage>().texture;

        itemBio.text = findItemBio(this.name);
    }

    // Function that returns an item bio depending on the name of the item. As
    // more items get added, this function will get longer and messier.
    string findItemBio(string itemName)
    {
        string bio;

        if (itemName == "Yellow Cube") {
            bio = "A cube made from a strange, yellow material. "
                + "Further investigation required.";

            return bio;
        } else if (itemName == "Green Cube") {
            bio = "You've heard of green cubes in the old sagas. "
                + "Could there be truth to the legends?";

            return bio;
        } else {
            return "ITEM UNKNOWN";
        }
    }
}
