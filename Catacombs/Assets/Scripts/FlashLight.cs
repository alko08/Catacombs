using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashLight : MonoBehaviour
{
    private bool isOn;
    public GameObject lightSource;
    public AudioSource clickSound;
    private TextMeshProUGUI textMeshPro;
    public float charge;
    inventoryScript inventory;

    // // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        textMeshPro = GameObject.FindWithTag("FlashlightCharge").GetComponent<TextMeshProUGUI>();
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
        if (Mathf.CeilToInt(charge*100) <= 0) {
            if (inventory.containsBattery()) {
                charge = 1f;
                inventory.removeBook("Battery");
            } else {
                charge = 0f;
                lightSource.SetActive(false);
                clickSound.Play();
                isOn = false; 
            }
        }
    }

    // FixedUpdate gets called once per tick (basically).
    void FixedUpdate()
    {
        if (isOn) {
            // charge -= .01f;
            charge -= .0002f;
        }
    }
}
