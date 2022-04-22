using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    private GameObject speakerObject;
    private Transform player;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("MainCamera").transform;
        speakerObject = this.gameObject.transform.GetChild(0).gameObject;
        // rb = speakerObject.GetComponent<Rigidbody>();
        speakerObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Speaker")) {
            throwSpeaker();
        }
    }

    void throwSpeaker() {
        GameObject clone;
        clone = Instantiate(speakerObject, player.position, transform.rotation);
        clone.SetActive(true);
        rb = clone.GetComponent<Rigidbody>();
        rb.velocity = player.forward * 10; //new Vector3(10, 0, 0);
        StartCoroutine(despawnCoroutine(clone));
        
        // this.gameObject.transform.position = player.position + new Vector3(0f, 1f, 0f);
        // speakerObject.SetActive(true);
        // rb.velocity = new Vector3(10, 0, 0);
    }

    IEnumerator despawnCoroutine(GameObject c) {
        yield return new WaitForSeconds(10f);
        Destroy(c);
    }
}
