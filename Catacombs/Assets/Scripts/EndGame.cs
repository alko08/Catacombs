using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Transform lookAt;
    // Creating variable to store inventoryScript.
    inventoryScript inventory;
    bool nearPlayer, pickedUp;
    GameObject player, crosshair, black, endText, art, mainCamera, 
        notifications, particles; // fog
    FirstPersonCamera FPCam;
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPC;
    CharacterController CControler;

    // Start by storing values in variables.
    void Start()
    {
        nearPlayer = false;
        pickedUp = false;
        art = transform.GetChild(0).gameObject;
        particles = transform.GetChild(1).gameObject;

        crosshair = GameObject.FindWithTag("Crosshair").transform.GetChild(0).gameObject;
        endText = GameObject.FindWithTag("Crosshair").transform.GetChild(1).gameObject;
        notifications = GameObject.FindWithTag("Notifications");

        player = GameObject.FindWithTag("Player");
        mainCamera = GameObject.FindWithTag("MainCamera");
        FPCam = player.transform.GetChild(0).gameObject.GetComponent<FirstPersonCamera>();
        FPC = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        CControler = player.GetComponent<CharacterController>();
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        
        GameObject fadeToBlack = GameObject.FindWithTag("FadeToBlack");
        black = fadeToBlack.transform.GetChild(0).gameObject;
        // fog = fadeToBlack.transform.GetChild(1).gameObject;
    }

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        nearPlayer = dist < 5f;
    }

    private void FixedUpdate() {
        if (pickedUp) {
            // Vector3 relativePos = lookAt.position - player.transform.position;
            mainCamera.transform.Translate(0, 0, Time.deltaTime / 1.5f);
            // Quaternion toRotation = Quaternion.LookRotation(relativePos);
            Quaternion toRotation = Quaternion.LookRotation(lookAt.position);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, toRotation, 1 * Time.deltaTime );
        }
    }

    void HitByRay() {
        if (!nearPlayer) {
            ExitByRay();
        }else if (!inventory.isOpen && !pickedUp) {
            crosshair.SetActive(true);
            endText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                doPickup();
            }
        }
    }

    void ExitByRay() {
        crosshair.SetActive(false);
        endText.SetActive(false);
    }

    // Function for handling things that happen when an object is collected by
    // the player.
    void doPickup()
    {
        pickedUp = true;
        FPCam.pickedUp = true;

        FPC.enabled = false;
        CControler.enabled = false;

        crosshair.SetActive(false);
        endText.SetActive(false);
        art.SetActive(false);
        notifications.SetActive(false);
        
        particles.SetActive(true);
        mainCamera.transform.DetachChildren();
        StartCoroutine(endCoroutine());
    }

    IEnumerator endCoroutine() {
        black.SetActive(true);
        // fog.SetActive(true);
        LeanTween.alpha (black.GetComponent<RectTransform>(), 1f, 10f).setEase(LeanTweenType.linear);
        // LeanTween.alpha (fog.GetComponent<RectTransform>(), .5f, 5f).setEase(LeanTweenType.linear);
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("WinScene");
    }
}
