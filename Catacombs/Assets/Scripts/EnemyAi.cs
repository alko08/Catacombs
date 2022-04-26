using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAi : MonoBehaviour
{
    public LayerMask whatIsPlayer;
    public float sightRange, warningRange, attackRange, walkSpeed, runSpeed;
    public bool isOnFloor, isInSight, seeHiding, moving;
    public GameObject hunting, warning;
    public AudioSource chase_audio_source;
    public float chase_volume = 0.0f;
    public Transform[] patrolPoints;
    
    private NavMeshAgent agent;
    private Ray sight0, sight1, sight2, sight3;
    private bool seePlayer, hunted, playerInAttackRange, playerInWarningRange, 
        walkPointSet, seeSpeaker, lastSeen, playerInSightRange, seeLight, 
        lookingThroughGlass, listened;
    private int patrolSpot;
    private Vector3 walkPoint;
    private Transform player, speaker;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPC;
    private Animator monsterAnimator;
    private FlashLight flash;

    private void Awake()
    {
        lookingThroughGlass = false;
        seeHiding = false;
        lastSeen = false;
        moving = true;
        seeSpeaker = false;
        hunted = false;
        patrolSpot = -1;
        seePlayer = false;
        player = GameObject.FindWithTag("Player").transform;
        FPC = player.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        agent = GetComponent<NavMeshAgent>();
        monsterAnimator = gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>();
        flash = GameObject.FindWithTag("Flashlight").GetComponent<FlashLight>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInWarningRange = Physics.CheckSphere(transform.position, warningRange, whatIsPlayer);
        playerInAttackRange = (seePlayer && !FPC.hiding) || (playerInWarningRange && FPC.sprinting) ||
            (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer) && !FPC.hiding);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        seeLight = flash.isOn && playerInSightRange && isInSight;

        seeHiding = false;
        if (moving) {
            lookingThroughGlass = false;
            if (lastSeen && FPC.hiding) {
                seeHiding = true;
            } else if (seeLight && FPC.hiding) {
                seeHiding = true;
            }
            
            if (seeSpeaker) {
                LookAtSpeaker();
            } else if (seeHiding) {
                ChasePlayer();
            } else if (!isOnFloor && seeLight) {
                LookAtLight();
            }else if ((!isOnFloor || !playerInAttackRange)) {
                // Debug.Log("Patrolling");
                Patroling();

                if (chase_audio_source.isPlaying) {
                    if (chase_volume > 0.0f) {
                        chase_volume -= (Time.deltaTime / 6);
                        chase_audio_source.volume = chase_volume;
                    }
                    else {
                        chase_audio_source.Stop();
                    }
                }
            } else {
                // Debug.Log("Chaseing");
                ChasePlayer();
                if (!chase_audio_source.isPlaying) {
                    chase_audio_source.volume = 0.0f;
                    chase_audio_source.Play();
                }

                if (chase_volume < 1.0f) {
                    chase_volume += (Time.deltaTime / 3);
                    chase_audio_source.volume = chase_volume;
                }
            }
        } else {
            agent.SetDestination(transform.position);
        }
    }

    private void FixedUpdate(){
        lastSeen = seePlayer;
        seePlayer = false;

        if (isOnFloor && isInSight && !FPC.hiding) {
            RaycastHit rayHit0, rayHit1, rayHit2, rayHit3;
            sight0.origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            sight0.direction = player.transform.position - sight0.origin;
            sight1.origin = sight0.origin + new Vector3(0f, 1f, 0f);
            sight1.direction = player.transform.position - sight0.origin - new Vector3(0f, .5f, 0f);
            sight2.origin = sight1.origin + new Vector3(0f, 1f, 0f);
            sight2.direction = player.transform.position - sight0.origin - new Vector3(0f, 1.25f, 0f);
            sight3.origin = sight2.origin + new Vector3(0f, 1f, 0f);
            sight3.direction = player.transform.position - sight0.origin - new Vector3(0f, 2.2f, 0f);
            

            if (Physics.Raycast(sight0, out rayHit0, sightRange)) {
                Debug.DrawLine(sight0.origin, rayHit0.point, Color.white);
                seePlayer = rayHit0.collider.tag == "Player";
                // Debug.Log("0:" + rayHit0.collider.name);
            }

            if (!seePlayer && Physics.Raycast(sight1, out rayHit1, sightRange)) {
                Debug.DrawLine(sight1.origin, rayHit1.point, Color.white);
                seePlayer = rayHit1.collider.tag == "Player";
                // Debug.Log("1:" + rayHit1.collider.name);
            }

            if (!seePlayer && Physics.Raycast(sight2, out rayHit2, sightRange)) {
                Debug.DrawLine(sight2.origin, rayHit2.point, Color.white);
                seePlayer = rayHit2.collider.tag == "Player";
                // Debug.Log("2:" + rayHit2.collider.name);
            }
            
            if (!seePlayer && Physics.Raycast(sight3, out rayHit3, sightRange)) {
                Debug.DrawLine(sight3.origin, rayHit3.point, Color.white);
                seePlayer = rayHit3.collider.tag == "Player";
                // Debug.Log("3:" + rayHit3.collider.name);
            }
            // Debug.Log("See Player: " + seePlayer);
        }

        if (lookingThroughGlass) {
            Vector3 relativePos = player.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime );
        }
    }

    private void Patroling()
    {
        if (!hunted) {
            GetComponent<NavMeshAgent>().speed = walkSpeed;
        }
        
        hunting.SetActive(false);
        if (playerInWarningRange) {
            warning.SetActive(true);
        } else {
            warning.SetActive(false);
        }

        
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude <= 3f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        if (hunted) {
            hunted = false;
            listened = true;
            monsterAnimator.SetTrigger("listen");
            moving = false;
            StartCoroutine(notMovingCoroutine());
        } else if (moving && !listened) {
            patrolSpot++;
            if (patrolSpot >= patrolPoints.Length) {
                patrolSpot = 0;
            }
        }

        if (moving) {
            walkPointSet = true;
            listened = false;
            walkPoint = patrolPoints[patrolSpot].position;
        }
    }

    private void ChasePlayer()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
        hunting.SetActive(true);
        warning.SetActive(false);
        
        // Debug.Log(player.position + "");
        agent.SetDestination(player.position);
        walkPoint = player.position;
        walkPointSet = true;
        hunted = true;
    }

    private void LookAtLight()
    {
        lookingThroughGlass = true;
        walkPointSet = false;
        hunted = true;
        
        hunting.SetActive(false);
        if (playerInWarningRange) {
            warning.SetActive(true);
        } else {
            warning.SetActive(false);
        }

        agent.SetDestination(player.position);
        Vector3 distanceToWalkPoint = transform.position - player.position;
        if (distanceToWalkPoint.magnitude <= 10f) {
            monsterAnimator.SetTrigger("listen");
            moving = false;
            StartCoroutine(notMovingCoroutine());
        }
    }

    private void LookAtSpeaker()
    {
        walkPointSet = false;
        hunted = true;

        hunting.SetActive(false);
        if (playerInWarningRange) {
            warning.SetActive(true);
        } else {
            warning.SetActive(false);
        }

        agent.SetDestination(speaker.position);
    }

    public void attackSpeaker(Transform speak)
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
        speaker = speak;
        seeSpeaker = true;
        agent.SetDestination(speaker.position);
    }

    public bool destroySpeaker(int count)
    {
        Vector3 distanceToWalkPoint = transform.position - speaker.position;
        seeSpeaker = distanceToWalkPoint.magnitude > 3f && count <= 20;
        if (!seeSpeaker) {
            speaker = null;
            moving = false;
            monsterAnimator.SetTrigger("stomp");
            StartCoroutine(notMovingCoroutine());
        }
        return seeSpeaker;
    }

    IEnumerator notMovingCoroutine() {
        monsterAnimator.SetBool("walking", false);
        yield return new WaitUntil(() => monsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("StartWalk"));
        monsterAnimator.SetBool("walking", true);
        moving = true;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, warningRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
