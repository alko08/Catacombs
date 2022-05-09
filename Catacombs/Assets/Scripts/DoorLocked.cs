
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLocked : MonoBehaviour
{
    bool nearPlayer, playerFar, isLocked;
    GameObject player, crosshair;
    FirstPersonCamera FPCam;
    Animator doorAnimator;
    BoxCollider doorCollider;
    public bool isExit = false;
    private inventoryScript inventory;
    private FlashLight flashlight;
    public int currMission;


    // Start by storing values in variables.
    void Start()
    {
        isLocked = true;
        flashlight = GameObject.FindWithTag("Flashlight").GetComponent<FlashLight>();
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        crosshair = GameObject.FindWithTag("Crosshair").transform.GetChild(0).gameObject;
        player = GameObject.FindWithTag("Player");
        FPCam = player.transform.GetChild(0).gameObject.GetComponent<FirstPersonCamera>();
        nearPlayer = false;
        doorAnimator = gameObject.GetComponent<Animator>();
        doorCollider = gameObject.GetComponent<BoxCollider>();

        if (currMission == 4) {
            inventory.addTask("Read the book beside the table.");
        }
    }

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        nearPlayer = dist < 5f;
        playerFar = dist > 7f;
        if (playerFar) {
            doorAnimator.SetBool("PlayerFar", true);
            doorCollider.enabled = true;
        } else {
            doorAnimator.SetBool("PlayerFar", false);
        }
    }

    void HitByRay() {
        if (!nearPlayer) {
            ExitByRay();
        } else {
            crosshair.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0) && (!isLocked || inventory.hasKey)) {
                ChangeDoor();
            } else if (Input.GetKeyDown(KeyCode.Mouse0) && (isLocked || !(inventory.hasKey))) {
                printLockedMessage();
            }
        }
    }

    void ExitByRay() {
        crosshair.SetActive(false);
    }

    // Function for handling things that happen when an object is collected by
    // the player.
    void ChangeDoor()
    {
        if (inventory.hasKey) {
            isLocked = false;
            inventory.removeBook("keyRing");
        }
        
        crosshair.SetActive(false);
        doorAnimator.SetTrigger("ChangeDoorState");
        doorCollider.enabled = false;
        
        if (isExit) {
            StartCoroutine(NextSceneCoroutine());
        }

        if (currMission == 4) {
            inventory.removeTask("Find the key at the center of the maze.");
            inventory.addTask("Get to the door and move onto the next level.");
        }
    }

    IEnumerator NextSceneCoroutine() {
        flashlight.updateVariables();
        inventory.updateVariables();
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void printLockedMessage()
    {
        if (currMission == 0) {
            GameObject.Find("missionManager").GetComponent<mission_00>().print_doorMessage();
        } else if (currMission == 1) {
            GameObject.Find("missionManager").GetComponent<mission_01>().print_doorMessage();
        } else if (currMission == 2) {
            GameObject.Find("missionManager").GetComponent<mission_02>().print_doorMessage();
        } else if (currMission == 3) {
            GameObject.Find("missionManager").GetComponent<mission_03>().print_doorMessage();
        }
    }
}
