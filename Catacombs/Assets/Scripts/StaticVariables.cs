using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    public static int batteryNum;
    public static float batteryVal;
    public static int speakerNum;
    public static bool walkWASD, walkARROW, jump, interact, flashlight, flashlight2, 
        inventory, crouch, crouch2, sprint, speaker, changed, controls;
    
    static StaticVariables()
    {
        batteryNum = 0;
        batteryVal = 1f;
        speakerNum = 0;

        walkWASD = false; 
        walkARROW = false; 
        jump = false; 
        interact = false;
        flashlight = false;
        flashlight2 = false;
        inventory = false;
        crouch = false;
        crouch2 = false;
        sprint = false;
        speaker = false;
        changed = true;
        controls = true;
    }
}
