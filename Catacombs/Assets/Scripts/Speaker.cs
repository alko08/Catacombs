using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    private GameObject speakerObject;
    private Transform player;
    private Rigidbody rb;
    private EnemyAi monster;
    private Animator monsterAnim;
    private int count;
    private bool canThrow;
    private inventoryScript inventory;

    // Start is called before the first frame update
    void Start()
    {
        canThrow = true;
        count = 0;
        monster = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
        monsterAnim = GameObject.FindWithTag("Enemy").transform.GetChild(1).gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("MainCamera").transform;
        speakerObject = transform.GetChild(0).gameObject;
        speakerObject.SetActive(false);
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Speaker") && canThrow && inventory.containsSpeaker()) {
            throwSpeaker();
        }
    }

    void throwSpeaker() {
        canThrow = false;
        inventory.removeSpeaker();
        GameObject clone;
        clone = Instantiate(speakerObject, player.position, transform.rotation);
        clone.SetActive(true);
        rb = clone.GetComponent<Rigidbody>();
        clone.GetComponent<AudioSource>().Play();
        rb.velocity = player.forward * 15; //new Vector3(10, 0, 0);
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
        yield return new WaitUntil(() => monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stomp"));
        yield return new WaitForSeconds(1.4f);
        c.GetComponent<AudioSource>().Stop();
        c.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(.1f);
        // c.GetComponent<MeshRenderer>().enabled = false;
        Destroy(c);
        canThrow = true;
    }
}
