using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterScript : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.tag);
        if (other.transform.tag == "Player") {
            SceneManager.LoadScene("LoseScene");
        } 
    }
}
