using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class Tutorial0 : MonoBehaviour
{
    private TextMeshProUGUI text;
    public GameObject webGL, normal;

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

    IEnumerator TutorialCoroutine() {
        yield return new WaitForSeconds(1f);
        text.SetText("Press [W,A,S,D] to walk.");
        yield return new WaitUntil(() => Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"));

        yield return new WaitForSeconds(2f);
        text.SetText("");
        yield return new WaitForSeconds(2f);

        text.SetText("Press [left click] to interact and pickup object.");
        yield return new WaitUntil(() => Input.GetButtonDown("Fire1"));

        yield return new WaitForSeconds(2f);
        text.SetText("");
        yield return new WaitForSeconds(2f);
        
        text.SetText("Grab the key and exit when ready.");
    }
}