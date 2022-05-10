using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class Tutorial0 : MonoBehaviour
{
    private TextMeshProUGUI text;
    public GameObject webGL, normal;
    private string message;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(TutorialCoroutine());
        if (Application.platform != RuntimePlatform.WebGLPlayer) {
            webGL.SetActive(false);
        } else {
            normal.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0) {
            text.SetText(message);
        } else {
            text.SetText("");
        }
    }

    IEnumerator TutorialCoroutine() {
        yield return new WaitForSeconds(1f);
        message = "Press [W,A,S,D] to walk.";
        yield return new WaitUntil(() => Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"));

        yield return new WaitForSeconds(2f);
        message = "";
        yield return new WaitForSeconds(2f);

        message = "Press [left click] to interact and pickup object.";
        yield return new WaitUntil(() => Input.GetButtonDown("Fire1"));

        yield return new WaitForSeconds(2f);
        message = "";
        yield return new WaitForSeconds(2f);
        
        message = "Grab the key and exit when ready.";
    }
}