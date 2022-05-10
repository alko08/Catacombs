using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public static class StaticVariables
{
    public static int batteryNum;
    public static float batteryVal;
    public static int speakerNum;
    public static bool walkWASD, jump, interact, flashlight, 
        inventory, crouch, crouch2, sprint, speaker, changed, controls;
    
    static StaticVariables()
    {
        batteryNum = 0;
        batteryVal = 1f;
        speakerNum = 0;

        walkWASD = false;
        jump = false; 
        interact = false;
        flashlight = false;
        inventory = true;
        crouch = false;
        crouch2 = false;
        sprint = false;
        speaker = false;
        changed = true;
        controls = true;
    }
}
