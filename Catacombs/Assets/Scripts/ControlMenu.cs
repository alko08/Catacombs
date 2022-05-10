using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ControlMenu : MonoBehaviour
{
    void Start()
    {
        transform.GetChild(0).GetChild(4).gameObject.GetComponent<Toggle>().isOn = StaticVariables.controls;
        transform.GetChild(0).GetChild(5).gameObject.GetComponent<Toggle>().isOn = StaticVariables.walkWASD;
        transform.GetChild(0).GetChild(6).gameObject.GetComponent<Toggle>().isOn = StaticVariables.sprint;
        transform.GetChild(0).GetChild(7).gameObject.GetComponent<Toggle>().isOn = StaticVariables.jump;
        transform.GetChild(0).GetChild(8).gameObject.GetComponent<Toggle>().isOn = StaticVariables.crouch;
        if (Application.platform != RuntimePlatform.WebGLPlayer) {
            transform.GetChild(0).GetChild(9).gameObject.GetComponent<Toggle>().isOn = StaticVariables.crouch2;
        } else {
            transform.GetChild(0).GetChild(9).gameObject.SetActive(false);
        }
        
        transform.GetChild(0).GetChild(10).gameObject.GetComponent<Toggle>().isOn = StaticVariables.interact;
        transform.GetChild(0).GetChild(11).gameObject.GetComponent<Toggle>().isOn = StaticVariables.flashlight;
        transform.GetChild(0).GetChild(12).gameObject.GetComponent<Toggle>().isOn = StaticVariables.inventory;
        transform.GetChild(0).GetChild(13).gameObject.GetComponent<Toggle>().isOn = StaticVariables.speaker;
    }

    public void updateControls (bool c) {
        StaticVariables.controls = c;
        StaticVariables.changed = true;
    }
    public void updateWalkWASD (bool w) {
        StaticVariables.walkWASD = w;
        StaticVariables.changed = true;
    }
    public void updateJump (bool j) {
        StaticVariables.jump = j;
        StaticVariables.changed = true;
    }
    public void updateInteract (bool i) {
        StaticVariables.interact = i;
        StaticVariables.changed = true;
    }
    public void updateFlashlight (bool f) {
        StaticVariables.flashlight = f;
        StaticVariables.changed = true;
    }
    public void updateInventory (bool i) {
        StaticVariables.inventory = i;
        StaticVariables.changed = true;
    }
    public void updateCrouch (bool c) {
        StaticVariables.crouch = c;
        StaticVariables.changed = true;
    }
    // public void updateCrouch2 (bool c) {
    //     StaticVariables.crouch2 = c;
    //     StaticVariables.changed = true;
    // }
    public void updateSprint (bool s) {
        StaticVariables.sprint = s;
        StaticVariables.changed = true;
    }
    public void updateSpeaker (bool s) {
        StaticVariables.speaker = s;
        StaticVariables.changed = true;
    }
}
