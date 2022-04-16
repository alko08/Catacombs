// Prints the name of the object camera is directly looking at
using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour
{
    Camera cam;
    Transform last;
    public bool pickedUp;
    // RaycastHit last;

    // public mission_01 missionScript;

    void Start()
    {
        pickedUp = false;
        last = this.transform;
        cam = GetComponent<Camera>();
        // Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        // Physics.Raycast(transform.position, -Vector3.up, out last);

        // missionScript = GameObject.Find("missionManager").GetComponent<mission_01>();
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;        
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        if (Physics.Raycast(ray, out hit, 7, layerMask)) {
            // print("I'm looking at " + hit.transform.name);
            if (hit.transform.tag == "pickup") {
                pickedUp = false;
                hit.transform.SendMessage("HitByRay");
            }
            else if (hit.transform.tag == "npc") {
                hit.transform.SendMessage("HitByRay");
            }

            if (last != hit.transform && last.tag == "pickup" && !pickedUp) 
                last.SendMessage("ExitByRay");
            if (last != hit.transform && last.tag == "npc")
                last.SendMessage("ExitByRay");
            last = hit.transform;
        } else {
            if (last.tag == "pickup" && !pickedUp) {
                last.SendMessage("ExitByRay");
                last = this.transform;
            }
            if (last.tag == "npc") {
                last.SendMessage("ExitByRay");
                last = this.transform;
            }
            // print("I'm looking at nothing!");
        }
            
        // if (Physics.Raycast(ray, out hit)) {
        //     // if (last.transform != hit.transform) {
        //     //     last.transform.SendMessage("ExitByRay");
        //         hit.transform.SendMessage("HitByRay");
                // last = hit;
            // }
        // }
            
    }

    // // Function to prevent player from moving past stage while level is
    // // incomplete. 
    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Collision detected");
        
    //     if (collision.gameObject.name == "doorBarrier") {
    //         missionScript.print_BarrierText();
    //     }
    // }
}