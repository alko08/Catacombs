using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterScript : MonoBehaviour
{
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPC;
    // Start is called before the first frame update
    void Start()
    {
        FPC = GameObject.FindWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.transform.tag);
        if (other.transform.tag == "Player" && !FPC.hiding) {
            SceneManager.LoadScene("LoseScene");
        } 
    }
}
