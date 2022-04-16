using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blythe : MonoBehaviour
{
    bool nearPlayer;
    GameObject player, crosshair;
    FirstPersonCamera FPCam;
    Animator blytheAnimator;

    // Start by storing values in variables.
    void Start()
    {
        crosshair = GameObject.FindWithTag("Crosshair").transform.GetChild(0).gameObject;
        player = GameObject.FindWithTag("Player");
        FPCam = player.transform.GetChild(0).gameObject.GetComponent<FirstPersonCamera>();
        nearPlayer = false;
        blytheAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        nearPlayer = dist < 5f;
    }

    void HitByRay() {
        if (!nearPlayer) {
            ExitByRay();
        }
        crosshair.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            talk();
        }
    }

    void ExitByRay() {
        crosshair.SetActive(false);
    }

    // Function for handling things that happen when an object is collected by
    // the player.
    void talk()
    {
        crosshair.SetActive(false);
        blytheAnimator.SetTrigger("talk_trigger");

    }
}
