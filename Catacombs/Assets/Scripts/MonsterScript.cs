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
    private bool foundPlayer, isUnder, bigSize;
    private EnemyAi monsterAI;
    // Start is called before the first frame update
    void Start()
    {
        bigSize = false;
        isUnder = false;
        foundPlayer = false;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        FPC = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        CControler = player.GetComponent<CharacterController>();
        monsterAnimator = gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>();

        GameObject fadeToBlack = GameObject.FindWithTag("FadeToBlack");
        black = fadeToBlack.transform.GetChild(0).gameObject;
        fog = fadeToBlack.transform.GetChild(1).gameObject;

        monsterAI = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
    }

    private void FixedUpdate() {
        if (foundPlayer && !isUnder) {
            player.transform.Translate(0, 0, Time.deltaTime / 1.5f);
        } else if (foundPlayer) {
            // player.transform.Translate(0, 0, Time.deltaTime / 1.5f);
        }

        if (monsterAI.seeHiding && !bigSize) {
            bigSize = true;
            GetComponent<CapsuleCollider>().radius = 5f;
        } else if (!monsterAI.seeHiding && bigSize) {
            bigSize = false;
            GetComponent<CapsuleCollider>().radius = 3f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.transform.tag);
        if (other.transform.tag == "Player" && (!FPC.hiding || monsterAI.seeHiding)) {
            if (!FPC.hiding) {
                monsterAnimator.SetTrigger("attack");
                player.transform.LookAt(gameObject.transform.GetChild(2));
                StartCoroutine(attackCoroutine());
            } else {
                isUnder = true;
                monsterAnimator.SetTrigger("attackUnder");
                player.transform.LookAt(gameObject.transform.GetChild(3));
                StartCoroutine(attackUnderCoroutine());
            }

            foundPlayer = true;
            FPC.enabled = false;
            agent.enabled = false;
            CControler.enabled = false;
            gameObject.GetComponent<EnemyAi>().enabled = false;
            gameObject.transform.LookAt(other.transform);
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

    IEnumerator attackUnderCoroutine() {
        yield return new WaitUntil(() => monsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("UnderDeskAttack"));
        black.SetActive(true);
        fog.SetActive(true);
        LeanTween.alpha (black.GetComponent<RectTransform>(), 1f, 2f).setEase(LeanTweenType.linear);
        LeanTween.alpha (fog.GetComponent<RectTransform>(), .5f, 2f).setEase(LeanTweenType.linear);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LoseScene");
    }
}
