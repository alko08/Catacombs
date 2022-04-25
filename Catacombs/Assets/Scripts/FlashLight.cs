using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using TMPro;

public class FlashLight : MonoBehaviour
{
    public bool isOn;
    public GameObject lightSource;
    public AudioSource clickSound;
    public AudioSource beepSound;
    // private TextMeshProUGUI textMeshPro;
    private GameObject chargeBar;
    private GameObject charge00, charge10, charge20, charge30, charge40, 
        charge50, charge60, charge70, charge80, charge90, charge100;
    public float charge;
    private GameObject beepText, monster;
    private inventoryScript inventory;
    private Ray light;
    // private EnemyAi monsterAI;

    // // Start is called before the first frame update
    void Start()
    {
        beepText = GameObject.FindWithTag("LoseBattery");
        beepText.SetActive(false);

        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        // textMeshPro = GameObject.FindWithTag("FlashlightCharge").GetComponent<TextMeshProUGUI>();
        chargeBar = GameObject.FindWithTag("FlashlightCharge");
        charge00 = chargeBar.transform.GetChild(0).gameObject;
        charge10 = chargeBar.transform.GetChild(1).gameObject;
        charge20 = chargeBar.transform.GetChild(2).gameObject;
        charge30 = chargeBar.transform.GetChild(3).gameObject;
        charge40 = chargeBar.transform.GetChild(4).gameObject;
        charge50 = chargeBar.transform.GetChild(5).gameObject;
        charge60 = chargeBar.transform.GetChild(6).gameObject;
        charge70 = chargeBar.transform.GetChild(7).gameObject;
        charge80 = chargeBar.transform.GetChild(8).gameObject;
        charge90 = chargeBar.transform.GetChild(9).gameObject;
        charge100 = chargeBar.transform.GetChild(10).gameObject;
        hideCharge();

        charge = StaticVariables.batteryVal;
        isOn = false;
        lightSource.SetActive(false);

        // monster = GameObject.FindWithTag("Enemy");
        // monsterAI = monster.GetComponent<EnemyAi>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2")) {
            if (!isOn && charge > 0f) {
                
                lightSource.SetActive(true);
                clickSound.Play();
                isOn = true;
                RenderSettings.fogDensity = 0.04f;
            } else if (isOn) {
                lightSource.SetActive(false);
                clickSound.Play();
                isOn = false;
                RenderSettings.fogDensity = 0.08f;
            }
        }

        if (Mathf.CeilToInt(charge*100) <= 50) {
            if (inventory.containsBattery()) {
                LeanTween.cancel(beepText);
                beepText.SetActive(true);
                beepText.transform.localScale = Vector3.one;
                LeanTween.scale(beepText, Vector3.zero, 1.5f).setOnComplete(hideLoseBattery);

                charge += .5f;
                beepSound.Play();
                inventory.removeBattery();
            } else if (isOn && Mathf.CeilToInt(charge*100) <= 0) {
                charge = 0f;
                lightSource.SetActive(false);
                clickSound.Play();
                isOn = false; 
            }
        }
        
        // textMeshPro.SetText(Mathf.CeilToInt(charge*100) + "%");
        int temp = Mathf.CeilToInt(charge*100);
        hideCharge();
        if (temp > 90) {
            charge100.SetActive(true);
        } else if (temp > 80) {
            charge90.SetActive(true);
        } else if (temp > 70) {
            charge80.SetActive(true);
        } else if (temp > 60) {
            charge70.SetActive(true);
        } else if (temp > 50) {
            charge60.SetActive(true);
        } else if (temp > 40) {
            charge50.SetActive(true);
        } else if (temp > 30) {
            charge40.SetActive(true);
        } else if (temp > 20) {
            charge30.SetActive(true);
        } else if (temp > 10) {
            charge20.SetActive(true);
        } else if (temp > 0) {
            charge10.SetActive(true);
        } else {
            charge00.SetActive(true);
        }
    }

    // FixedUpdate gets called once per tick (basically).
    void FixedUpdate()
    {
        if (isOn && Time.timeScale != 0) {
            // charge -= .01f;
            charge -= .0002f;
        }

        // int layerMask = 1 << 3;
        // layerMask = ~layerMask;
        // light.origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // light.direction = monster.transform.position - light.origin;
        // RaycastHit rayHit;
    
        // if (isOn && Physics.Raycast(light, out rayHit, 30f, layerMask)) {
        //     // print("I'm looking at " + rayHit.transform.name);
        //     Debug.DrawLine(light.origin, rayHit.point, Color.white);
        //     monsterAI.seeLight = rayHit.collider.tag == "Enemy";
        // } else {
        //     monsterAI.seeLight = false;
        // }
    }

    void hideCharge()
    {
        charge00.SetActive(false);
        charge10.SetActive(false);
        charge20.SetActive(false);
        charge30.SetActive(false);
        charge40.SetActive(false);
        charge50.SetActive(false);
        charge60.SetActive(false);
        charge70.SetActive(false);
        charge80.SetActive(false);
        charge90.SetActive(false);
        charge100.SetActive(false);
    }

    void hideLoseBattery() {
        beepText.SetActive(false);
    }
    
    public void updateVariables() {
        StaticVariables.batteryVal = charge;
    }
}
