using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript2 : MonoBehaviour
{   
    // Creating variable to store inventoryScript.
    inventoryScript inventory;
    bool nearPlayer;
    GameObject player, crosshair;
    FirstPersonCamera FPCam;

    // Start by storing values in variables.
    void Start()
    {
        crosshair = GameObject.FindWithTag("Crosshair");
        player = GameObject.FindWithTag("Player");
        FPCam = player.transform.GetChild(0).gameObject.GetComponent<FirstPersonCamera>();
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
            crosshair.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                doPickup();
            }
        }
    }

    void ExitByRay() {
        crosshair.SetActive(false);
    }

    // Function for handling things that happen when an object is collected by
    // the player.
    void doPickup()
    {
        crosshair.SetActive(false);
        gameObject.SetActive(false);
        inventory.addBook(this.name);
        FPCam.pickedUp = true;
        Destroy(this);
    }
}
