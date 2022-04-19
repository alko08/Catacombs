using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    public static int batteryNum;
    public static float batteryVal;

    static StaticVariables()
    {
        batteryNum = 0;
        batteryVal = 1f;
    }
}
