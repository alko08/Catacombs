using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorGlowScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Variables
    private GameObject myGlow;
    private inventoryScript inventory;
    public bool mousingOver;

    // On start, grab inventory. 
    void Start()
    {
        if (gameObject.name == "InventoryButton") {
            myGlow = GameObject.Find("selectUIParent").transform.GetChild(0).GetChild(1).gameObject;
        } else if (gameObject.name == "ObjectivesButton") {
            myGlow = GameObject.Find("selectUIParent").transform.GetChild(0).GetChild(0).gameObject;
        }

        myGlow.SetActive(false);
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        mousingOver = false;
    }

    void Update()
    {
        if ( (mousingOver) && (Input.GetKeyDown(KeyCode.Mouse0)) ) {
            mousingOver = false;
            myGlow.SetActive(false);
            
            if (gameObject.name == "InventoryButton") {
                // Debug.Log("Opening inventory");
                inventory.SelectToInventory();
            } else if (gameObject.name == "ObjectivesButton") {
                // Debug.Log("Opening tasks");
                inventory.SelectToTasks();
            }
        }
    }
    
    // While mousing over this object, activate glow. If click is detected,
    // open inventory. 
    public void OnPointerEnter(PointerEventData eventData)
    {
        mousingOver = true;
        myGlow.SetActive(true);
    }

    // When cursor leaves, deactivate glow. 
    public void OnPointerExit(PointerEventData eventData)
    {
        mousingOver = false;
        myGlow.SetActive(false);
    }
}
