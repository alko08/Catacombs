using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetVariables : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StaticVariables.batteryNum = 0;
        StaticVariables.batteryVal = 1f;
        StaticVariables.speakerNum = 0;
    }
}
