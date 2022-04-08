using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript2 : MonoBehaviour
{   
    // Creating variable to store inventoryScript.
    inventoryScript inventory;
    bool nearPlayer;
    GameObject player;

    // Start by storing values in variables.
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        nearPlayer = false;
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
    }

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        nearPlayer = dist < 5f;
    }
    
    // Have color change when mousing over and not mousing over object. If
    // the player clicks the object, they'll pick it up. 
    // void OnMouseOver()
    // {
    //     if (inventory.isOpen == false) {
    //         if (Input.GetKeyDown(KeyCode.Mouse0)) {
    //             doPickup();
    //         }
    //     }
    // }

    void HitByRay() {
        if (!nearPlayer) {
            ExitByRay();
        }else if (inventory.isOpen == false) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                doPickup();
            }
        }
    }

    void ExitByRay() {
        // Do nothing
    }

    // Function for handling things that happen when an object is collected by
    // the player.
    void doPickup()
    {
        gameObject.SetActive(false);
        inventory.addBook(this.name);
        Destroy(this);
    }
}
