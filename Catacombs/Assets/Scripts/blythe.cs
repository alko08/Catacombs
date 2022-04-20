using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blythe : MonoBehaviour
{
    bool nearPlayer;
    GameObject player, crosshair, talkText;
    FirstPersonCamera FPCam;
    Animator blytheAnimator;
    float dist;
    bool inventoryOpen;

    // Start by storing values in variables.
    void Start()
    {
        crosshair = GameObject.FindWithTag("Crosshair").transform.GetChild(0).gameObject;
        talkText = GameObject.Find("crosshair_talk");
        talkText.SetActive(false);
        player = GameObject.FindWithTag("Player");
        FPCam = player.transform.GetChild(0).gameObject.GetComponent<FirstPersonCamera>();
        nearPlayer = false;
        blytheAnimator = gameObject.GetComponent<Animator>();
        dist = Vector3.Distance(player.transform.position, transform.position);
        inventoryOpen = false;
    }

    void Update()
    {
        dist = Vector3.Distance(player.transform.position, transform.position);
        nearPlayer = (dist < 5f);

        inventoryOpen = (GameObject.Find("EventSystem").GetComponent<inventoryScript>().isOpen_select 
                       | GameObject.Find("EventSystem").GetComponent<inventoryScript>().isOpen
                       | GameObject.Find("EventSystem").GetComponent<inventoryScript>().isOpen_tasks);
    }

    void HitByRay() {
        if ((!nearPlayer) | (inventoryOpen)) {
            ExitByRay();
        }

        if (!inventoryOpen) {
            crosshair.SetActive(true);
            talkText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                talk();
            }
        }
    }

    void ExitByRay() {
        crosshair.SetActive(false);
        talkText.SetActive(false);
    }

    // Function for handling things that happen when an object is collected by
    // the player.
    void talk()
    {
        crosshair.SetActive(false);
        talkText.SetActive(false);
        blytheAnimator.SetTrigger("talk_trigger");
        GameObject.Find("missionManager").GetComponent<mission_00>().print_blytheTalk1();
    }
}
