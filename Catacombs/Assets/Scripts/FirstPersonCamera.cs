// Prints the name of the object camera is directly looking at
using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour
{
    Camera cam;
    Transform last;
    // RaycastHit last;

    void Start()
    {
        last = this.transform;
        cam = GetComponent<Camera>();
        // Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        // Physics.Raycast(transform.position, -Vector3.up, out last);
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;        
        if (Physics.Raycast(ray, out hit)) {
            // print("I'm looking at " + hit.transform.name);
            if (hit.transform.tag == "pickup") {
                hit.transform.SendMessage("HitByRay");
            }

            if (last != hit.transform && last.transform.tag == "pickup") 
                last.SendMessage("ExitByRay");
            last = hit.transform;
        } else {
            if (last.transform.tag == "pickup") {
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
}