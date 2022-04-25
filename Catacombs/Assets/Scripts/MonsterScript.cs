using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class MonsterScript : MonoBehaviour
{
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPC;
    private GameObject player, black, fog;
    private NavMeshAgent agent;
    private Animator monsterAnimator;
    private CharacterController CControler;
    private bool foundPlayer;
    // Start is called before the first frame update
    void Start()
    {
        foundPlayer = false;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        FPC = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        CControler = player.GetComponent<CharacterController>();
        monsterAnimator = gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>();

        GameObject fadeToBlack = GameObject.FindWithTag("FadeToBlack");
        black = fadeToBlack.transform.GetChild(0).gameObject;
        fog = fadeToBlack.transform.GetChild(1).gameObject;
    }

    private void FixedUpdate() {
        if (foundPlayer) {
            player.transform.Translate(0, 0, Time.deltaTime / 1.5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.transform.tag);
        if (other.transform.tag == "Player" && !FPC.hiding) {
            foundPlayer = true;
            FPC.enabled = false;
            agent.enabled = false;
            CControler.enabled = false;
            gameObject.GetComponent<EnemyAi>().enabled = false;
            gameObject.transform.LookAt(other.transform);
            player.transform.LookAt(gameObject.transform.GetChild(2));
            monsterAnimator.SetTrigger("attack");
            StartCoroutine(attackCoroutine());
        } 
    }

    IEnumerator attackCoroutine() {
        yield return new WaitUntil(() => monsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack"));
        black.SetActive(true);
        fog.SetActive(true);
        LeanTween.alpha (black.GetComponent<RectTransform>(), 1f, 1f).setEase(LeanTweenType.linear);
        LeanTween.alpha (fog.GetComponent<RectTransform>(), .5f, 1f).setEase(LeanTweenType.linear);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LoseScene");
    }
}
