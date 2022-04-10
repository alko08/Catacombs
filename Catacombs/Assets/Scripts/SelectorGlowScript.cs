using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorGlowScript : MonoBehaviour
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

    // void Update()
    // {
    //     if (mousingOver) {
    //         Debug.Log("The cursor entered the selectable UI element.");
    //     }
    // }
    
    // While mousing over this object, activate glow. If click is detected,
    // open inventory. 
    public void OnPointerEnter(PointerEventData eventData)
    {
        myGlow.SetActive(true);
    }

    // When cursor leaves, deactivate glow. 
    void OnMouseExit()
    {
        myGlow.SetActive(false);
    }
}
