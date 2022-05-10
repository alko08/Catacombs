using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowControls : MonoBehaviour
{
    private TextMeshProUGUI controlText;
    private string str;
    public bool walkWASD, jump, interact, flashlight, 
        inventory, crouch, crouch2, sprint, speaker, changed, controls;
    private int num;
    private RectTransform myRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        num = 0;
        myRectTransform = GetComponent<RectTransform>();
        str = "Press [q] or [esc] for settings.\n";
        controlText = GetComponent<TextMeshProUGUI>();
        controlText.SetText(str);
    }

    void Update()
    {
        changed = StaticVariables.changed;
        if (changed) {
            changed = false;
            StaticVariables.changed = false;
            str = "";
            num = 0;

            walkWASD = StaticVariables.walkWASD; 
            jump = StaticVariables.jump; 
            interact = StaticVariables.interact;
            flashlight = StaticVariables.flashlight;
            inventory = StaticVariables.inventory;
            crouch = StaticVariables.crouch;
            crouch2 = StaticVariables.crouch2;
            sprint = StaticVariables.sprint;
            speaker = StaticVariables.speaker;
            controls = StaticVariables.controls;

            if (controls) {
                str = "Press [q] or [esc] for settings.\n";
                num += 1;
            }
            if (walkWASD) {
                str += "Press [w,a,s,d] to move.\n";
                num += 1;
            }
            if (crouch) {
                str += "Press [c] to crouch.\n";
                num += 1;
            }
            if (crouch2) {
                str += "Press [ctrl] to crouch.\n";
                num += 1;
            }
            if (sprint) {
                str += "Press [shift] to run.\n";
                num += 1;
            }
            if (jump) {
                str += "Press [space] to jump.\n";
                num += 1;
            }
            if (interact) {
                str += "Press [left click] to interact.\n";
                num += 1;
            }
            if (flashlight) {
                str += "Press [right click] for flashlight.\n";
                num += 1;
            }
            if (inventory) {
                str += "Press [e] for inventory.\n";
                num += 1;
            }
            if (speaker) {
                str += "Press [f] to throw a music-maker.\n";
                num += 1;
            }

            controlText.SetText(str);
            myRectTransform.localPosition = new Vector3(485, -385, 0);
            myRectTransform.localPosition += new Vector3(0, 23, 0) * num;
        }
    }
}
