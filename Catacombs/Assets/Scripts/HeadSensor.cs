using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSensor : MonoBehaviour
{
    private int count;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPC;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        FPC = transform.parent.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        FPC.hiding = count != 0;
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag != "IgnoreObject") {
            count++;
        }  
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.tag != "IgnoreObject") {
            count--;
        }
    }
}
