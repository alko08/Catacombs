using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    private GameObject speakerObject;
    private Transform player;
    private Rigidbody rb;
    private EnemyAi monster;
    private int count;
    private bool canThrow;

    // Start is called before the first frame update
    void Start()
    {
        canThrow = true;
        count = 0;
        monster = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
        player = GameObject.FindWithTag("MainCamera").transform;
        speakerObject = this.gameObject.transform.GetChild(0).gameObject;
        // rb = speakerObject.GetComponent<Rigidbody>();
        speakerObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Speaker") && canThrow) {
            throwSpeaker();
        }
    }

    void throwSpeaker() {
        canThrow = false;
        GameObject clone;
        clone = Instantiate(speakerObject, player.position, transform.rotation);
        clone.SetActive(true);
        rb = clone.GetComponent<Rigidbody>();
        clone.GetComponent<AudioSource>().Play();
        rb.velocity = player.forward * 10; //new Vector3(10, 0, 0);
        monster.attackSpeaker(clone.transform);
        StartCoroutine(despawnCoroutine(clone));
        
        // this.gameObject.transform.position = player.position + new Vector3(0f, 1f, 0f);
        // speakerObject.SetActive(true);
        // rb.velocity = new Vector3(10, 0, 0);
    }

    IEnumerator despawnCoroutine(GameObject c) {
        yield return new WaitForSeconds(10f);
        if(!monster.destroySpeaker(0)) {
            StartCoroutine(destroyCoroutine(c));
        } else {
            count = 0;
            StartCoroutine(waitCoroutine(c));
        }
    }
    IEnumerator waitCoroutine(GameObject c) {
        yield return new WaitForSeconds(1f);
        if(!monster.destroySpeaker(count)) {
            StartCoroutine(destroyCoroutine(c));
        } else {
            count++;
            StartCoroutine(waitCoroutine(c));
        }
    }

    IEnumerator destroyCoroutine(GameObject c) {
        yield return new WaitForSeconds(1.8f);
        Destroy(c);
        canThrow = true;
    }
}
