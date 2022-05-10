using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class Tutorial : MonoBehaviour
{
    public bool nearDesk, endTutorial;

    private TextMeshProUGUI text;
    private bool endWalk; //started

    // Start is called before the first frame update
    void Start()
    {
        nearDesk = false;
        endTutorial = false;
        // started = false;
        endWalk = false;
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(TutorialCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        // if (!started) {
        if (endTutorial) {
            text.SetText("");
            Destroy(gameObject);
            // StartCoroutine(EndCoroutine());
        } else if (nearDesk) {
            endWalk = true;
            if (Application.platform != RuntimePlatform.WebGLPlayer) {
                text.SetText("Press [ctrl] or [c] to crouch and hide under objects.");
            } else {
                text.SetText("Press [c] to crouch and hide under objects.");
            }
        } else if (endWalk) {
            text.SetText("");
        }
        // }
    }

    IEnumerator TutorialCoroutine() {
        yield return new WaitForSeconds(1f);
        text.SetText("Press [right click] for flashlight.");
        yield return new WaitUntil(() => Input.GetButtonDown("Fire2"));

        yield return new WaitForSeconds(2f);
        if (!nearDesk) {
            text.SetText("");
        } 
        yield return new WaitForSeconds(2f);

        if (!nearDesk) {
            text.SetText("Press [shift] to sprint.");
        }
        yield return new WaitUntil(() => Input.GetKey(KeyCode.LeftShift));
        yield return new WaitForSeconds(2f);
        if (!nearDesk) {
            text.SetText("");
        }
        // yield return new WaitForSeconds(1f);
        // text.SetText("Press [W,A,S,D] or [Arrow Keys] to walk.");
        // yield return new WaitUntil(() => Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"));

        // yield return new WaitForSeconds(2f);
        // text.SetText("");
        // yield return new WaitForSeconds(2f);

        // text.SetText("Press [shift] to sprint.");
        // yield return new WaitUntil(() => Input.GetKey(KeyCode.LeftShift));
        // yield return new WaitForSeconds(2f);
        // text.SetText("");
    }

    // IEnumerator EndCoroutine() {
    //     started = true;
    //     text.SetText("");
        // yield return new WaitForSeconds(2f);
        // text.SetText("Press [right click] for flashlight and [left click] to interact.");
        // yield return new WaitUntil(() => Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1"));
        // yield return new WaitForSeconds(5f);
    //     Destroy(gameObject);
    // }
}