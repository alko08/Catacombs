using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorGlowScript : MonoBehaviour
{
    // Variables
    public GameObject myGlow;
    public inventoryScript inventory;

    // On start, grab inventory. 
    void Start()
    {
        myGlow.SetActive(false);
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
    }
    
    // While mousing over this object, activate glow. If click is detected,
    // open inventory. 
    void OnMouseOver()
    {
        Debug.Log("MouseOver == true");
        myGlow.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            inventory.SelectToInventory();
        }
    }

    // When cursor leaves, deactivate glow. 
    void OnMouseExit()
    {
        myGlow.SetActive(false);
    }
}
