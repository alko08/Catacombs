using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript : MonoBehaviour
{
    // Creating variables for colors. TO-DO: Change to shaders.
    Color greenTexture = Color.green;
    Color defaultTexture;

    // Creating variable to store the GameObject's renderer.
    MeshRenderer m_Renderer;
    
    // Creating variable to store inventoryScript.
    inventoryScript inventory;
    bool nearPlayer;
    GameObject player;

    // Start by storing values in variables.
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        nearPlayer = false;
        m_Renderer = GetComponent<MeshRenderer>();
        defaultTexture = m_Renderer.material.color;
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
    }

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        nearPlayer = dist < 3f;
    }
    
    // Have color change when mousing over and not mousing over object. If
    // the player clicks the object, they'll pick it up. 
    // void OnMouseOver()
    // {
    //     if (inventory.isOpen == false) {
    //         m_Renderer.material.color = greenTexture;

    //         if (Input.GetKeyDown(KeyCode.Mouse0)) {
    //             doPickup();
    //         }
    //     }
    // }

    // void OnMouseExit()
    // {
    //     m_Renderer.material.color = defaultTexture;
    // }
    void HitByRay() {
        if (!nearPlayer) {
            ExitByRay();
        }else if (inventory.isOpen == false) {
            m_Renderer.material.color = greenTexture;
            if (Input.GetKeyDown(KeyCode.Mouse0) && nearPlayer) {
                doPickup();
            }
        }
    }

    void ExitByRay() {
        m_Renderer.material.color = defaultTexture;
    }

    // Function for handling things that happen when an object is collected by
    // the player.
    void doPickup()
    {
        gameObject.SetActive(false);
        inventory.addBook(this.name);
        Destroy(this);

        // if (this.name == "bat_box") {
        //     // Do thing with battery timer.
        // } else {
        //     // Add the book to inventory.
        //     inventory.addBook(this.name);
        // }
    }
}
