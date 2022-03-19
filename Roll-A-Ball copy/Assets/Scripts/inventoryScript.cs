using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class inventoryScript : MonoBehaviour
{
    public bool inventoryOpen;
    public GameObject inventory;
    public GameObject details;
    public int numItems;
    public const int ARRAYSIZE = 12;

    // Textures.
    public Texture2D cube;
    public Texture2D cube_green;


    public GameObject[] inventoryItems;
    
    // Start is called before the first frame update
    void Start()
    {
        // Hiding stuff. 
        inventoryOpen = false;
        inventory.SetActive(false);
        for (int i = 0; i < ARRAYSIZE; i++) {
                inventoryItems[i].SetActive(false);
            }
        details.SetActive(false);

        updateNumItems();
    }

    void updateNumItems()
    {
        numItems = GameObject.Find("Player").GetComponent<PlayerController>().count;
    }

    // Test input detection to open/close inventory screen. Used while
    // using roll-a-ball as a base for implementation. 
    // 
    // TO-DO: Rework to fit into Catacombs player inputs.
    void OnFire()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            open_or_close();
        }
    }

    // Function for opening/closing inventory screen.
    void open_or_close()
    {
        if (inventoryOpen) {
            doClose();
        } else {
            doOpen();
        }

        inventoryOpen = !inventoryOpen;
    }

    void doClose()
    {
        updateNumItems();
        inventory.SetActive(false);

        for (int i = 0; i < numItems; i++) {
            inventoryItems[i].SetActive(false);
        }

        details.SetActive(false);
    }

    public void doOpen()
    {
        updateNumItems();
        inventory.SetActive(true);

        for (int i = 0; i < numItems; i++) {
            inventoryItems[i].SetActive(true);
        }

        details.SetActive(true);
    }

    public void updateInventory(GameObject pickup)
    {
        // doOpen();
        updateNumItems();

        if (inventoryOpen) {
            doOpen();
        }

        // Updating item name.
        inventoryItems[numItems - 1].name = pickup.name;

        // Updating textures.
        Debug.Log(pickup.GetComponent<Renderer>().material.name);

        if (pickup.GetComponent<Renderer>().material.name == "Pickup (Instance)") {
            
            Debug.Log("Yellow cube found");
            
            inventoryItems[numItems - 1].GetComponent<RawImage>().texture
                = cube;
        } else if (pickup.GetComponent<Renderer>().material.name == "Pickup_green (Instance)") {
            
            Debug.Log("Green cube found");

            inventoryItems[numItems - 1].GetComponent<RawImage>().texture
                = cube_green;
        }
    }
    
}
