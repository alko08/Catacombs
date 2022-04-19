
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
    }

    IEnumerator NextSceneCoroutine() {
        flashlight.updateVariables();
        inventory.updateVariables();
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
