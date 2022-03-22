using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public bool isOn = false;
    public GameObject lightSource;
    public AudioSource clickSound;
    // private bool notClicked = true;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     failSafe = true;
    // }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Flashlight")) {
            if (!isOn) {
                lightSource.SetActive(true);
                clickSound.Play();
                isOn = true;
            } else {
                lightSource.SetActive(false);
                clickSound.Play();
                isOn = false; 
            }
        }
    }
}
