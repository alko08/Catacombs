using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryScript : MonoBehaviour
{
    // Declaring Variables.
    public bool isOpen;
    public GameObject inventoryUI;  // Refers to the parent containing all
                                    // inventory elements.
    public int numItems;
    public GameObject[] inventoryList;

    // Item Textures.
    public Texture2D book1;
    
    // Begin by hiding all inventory UI elements.
    void Start()
    {
        isOpen = false;
        inventoryUI = GameObject.Find("inventoryUI");
        inventoryUI.SetActive(false);
        numItems = 0;
    }

    // On update, check if the [R] key was pressed. If it was, switch the
    // of the inventory.
    void Update()
    {
        if (Input.GetButtonDown("r")) {
            if (!isOpen) {
                inventoryUI.SetActive(true);
            } else {
                inventoryUI.SetActive(false);
            }

            isOpen = !isOpen;
        }
    }
}
