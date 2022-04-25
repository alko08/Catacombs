using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    public static int batteryNum;
    public static float batteryVal;
    public static int speakerNum;

    static StaticVariables()
    {
        batteryNum = 0;
        batteryVal = 1f;
        speakerNum = 0;
    }
}
