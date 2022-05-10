using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class Tutorial : MonoBehaviour
{
    public bool nearDesk, endTutorial;

    private TextMeshProUGUI text;
    private bool started, endWalk;
    private string message;

    // Start is called before the first frame update
    void Start()
    {
        nearDesk = false;
        endTutorial = false;
        started = false;
        endWalk = false;
        message = "";
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(TutorialCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!started && endTutorial) {
            StartCoroutine(EndCoroutine());
        } else if (!started && nearDesk) {
            endWalk = true;
            if (Application.platform != RuntimePlatform.WebGLPlayer) {
                message = "Press [ctrl] or [c] to crouch and hide under objects.";
            } else {
                message = "Press [c] to crouch and hide under objects.";
            }
        } else if (!started && endWalk) {
            message = "";
        }
        if (Time.timeScale != 0) {
            text.SetText(message);
        } else {
            text.SetText("");
        }
    }

    IEnumerator TutorialCoroutine() {
        yield return new WaitForSeconds(1f);
        message = "Press [right click] for flashlight.";
        yield return new WaitUntil(() => Input.GetButtonDown("Fire2"));

        yield return new WaitForSeconds(2f);
        if (!nearDesk && !started) {
            message = "";
        } 
        yield return new WaitForSeconds(2f);

        if (!nearDesk && !started) {
            message = "Press [shift] to sprint.";
        }
        yield return new WaitUntil(() => Input.GetKey(KeyCode.LeftShift));
        yield return new WaitForSeconds(2f);
        if (!nearDesk && !started) {
            message = "";
        }
    }

    IEnumerator EndCoroutine() {
        started = true;
        message = "";
        yield return new WaitForSeconds(2f);
        message = "Press [e] to open your inventory.";
        yield return new WaitUntil(() => Input.GetButtonDown("Inventory") || Input.GetButtonDown("Fire1"));
        Destroy(gameObject);
    }
}