using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashLight : MonoBehaviour
{
    private bool isOn;
    public GameObject lightSource;
    public AudioSource clickSound;
    private TextMeshPro textMeshPro;
    public float charge;

    // // Start is called before the first frame update
    void Start()
    {
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
        charge = 1f;
        isOn = false;
        lightSource.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Flashlight")) {
            if (!isOn && charge > 0f) {
                lightSource.SetActive(true);
                clickSound.Play();
                isOn = true;
            } else {
                lightSource.SetActive(false);
                clickSound.Play();
                isOn = false; 
            }
        }

        textMeshPro.SetText(Mathf.CeilToInt(charge*100) + "%");
    }

    // FixedUpdate gets called once per tick (basically).
    void FixedUpdate()
    {
        if (isOn) {
            charge -= .01f;
        }
    }
}
