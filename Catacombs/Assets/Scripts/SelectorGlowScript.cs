using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorGlowScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Variables
    public GameObject myGlow;
    public inventoryScript inventory;
    public bool mousingOver;

    // On start, grab inventory. 
    void Start()
    {
        myGlow.SetActive(false);
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        mousingOver = false;
    }

    void Update()
    {
        if ( (mousingOver) && (Input.GetKeyDown(KeyCode.Mouse0)) ) {
            myGlow.SetActive(false);
            inventory.SelectToInventory();
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
