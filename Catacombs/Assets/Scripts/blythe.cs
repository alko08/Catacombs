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
    bool dialogueOpen;
    public int currMission;

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
        dialogueOpen = false;
    }

    void Update()
    {
        dist = Vector3.Distance(player.transform.position, transform.position);
        nearPlayer = (dist < 5f);

        inventoryOpen = (GameObject.Find("EventSystem").GetComponent<inventoryScript>().isOpen_select 
                       | GameObject.Find("EventSystem").GetComponent<inventoryScript>().isOpen
                       | GameObject.Find("EventSystem").GetComponent<inventoryScript>().isOpen_tasks);

        dialogueOpen = GameObject.Find("missionManager").GetComponent<mission_00>().isOpen_dialogue;
    }

    void HitByRay() {
        if ((!nearPlayer) | (inventoryOpen)) {
            ExitByRay();
        }

        if (currMission == 1) {
            GameObject.Find("missionManager").GetComponent<mission_01>().print_noticeBlythe();
        }

        if ( (!inventoryOpen) && (!dialogueOpen) && (Time.timeScale != 0)) {
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

        if (currMission == 0) {
            GameObject.Find("missionManager").GetComponent<mission_00>().print_blytheTalk1();
        } else if (currMission == 1) {
            GameObject.Find("missionManager").GetComponent<mission_01>().print_blytheTalk();
        } else if (currMission == 2) {
            GameObject.Find("missionManager").GetComponent<mission_02>().print_blytheTalk();
        } else if (currMission == 3) {
            GameObject.Find("missionManager").GetComponent<mission_03>().print_blytheTalk();
        }
    }
}
