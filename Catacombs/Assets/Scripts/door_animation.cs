
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_animation : MonoBehaviour
{
    bool nearPlayer, playerFar;
    GameObject player, crosshair;
    FirstPersonCamera FPCam;
    Animator doorAnimator;
    BoxCollider doorCollider;

    // Start by storing values in variables.
    void Start()
    {
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
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
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
        crosshair.SetActive(false);
        doorAnimator.SetTrigger("ChangeDoorState");
        doorCollider.enabled = false;
    }
}
