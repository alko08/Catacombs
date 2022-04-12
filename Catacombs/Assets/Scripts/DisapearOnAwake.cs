using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapearOnAwake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine() {
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
