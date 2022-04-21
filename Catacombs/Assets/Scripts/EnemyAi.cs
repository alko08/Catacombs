
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsPlayer;
    public float sightRange, warningRange, attackRange, walkSpeed, runSpeed;
    public bool isOnFloor, isInSight; //seeLight;
    public GameObject hunting, warning;
    public AudioSource chase_audio_source;
    public float chase_volume = 0.0f;
    public Transform[] patrolPoints;
    
    private Ray sight0, sight1, sight2, sight3;
    private bool seePlayer, hunted, playerInSightRange, playerInWarningRange, walkPointSet;
    private int patrolSpot;
    private Vector3 walkPoint;
    private Transform player;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPC;

    private void Awake()
    {
        hunted = false;
        patrolSpot = -1;
        seePlayer = false;
        player = GameObject.FindWithTag("Player").transform;
        FPC = player.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInWarningRange = Physics.CheckSphere(transform.position, warningRange, whatIsPlayer) && isOnFloor;
        playerInSightRange = (seePlayer && !FPC.hiding) || (playerInWarningRange && FPC.sprinting) ||
            (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer) && !FPC.hiding);

        if (!isOnFloor || !playerInSightRange) {
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
    }

    private void FixedUpdate(){
        seePlayer = false;

        if (playerInWarningRange && isInSight) {
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
        // Random Patrol Point on Navmesh
        // float radius = walkPointRange;
        // Vector3 randomDirection = Random.insideUnitSphere * radius;
        // randomDirection += transform.position;
        // NavMeshHit hit;
        // Vector3 finalPosition = Vector3.zero;
        // if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
        //     finalPosition = hit.position;            
        // }
        // walkPointSet = true;
        // walkPoint = finalPosition;
        if (hunted) {
            hunted = false;
        } else {
            patrolSpot++;
            if (patrolSpot >= patrolPoints.Length) {
                patrolSpot = 0;
            }
        }

        walkPointSet = true;
        walkPoint = patrolPoints[patrolSpot].position;
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

    // private void AttackPlayer()
    // {
    //     //Make sure enemy doesn't move
    //     agent.SetDestination(transform.position);

    //     transform.LookAt(player);

    //     if (!alreadyAttacked)
    //     {
    //         ///Attack code here
    //         Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
    //         rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
    //         rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    //         ///End of attack code

    //         alreadyAttacked = true;
    //         Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //     }
    // }
    // private void ResetAttack()
    // {
    //     alreadyAttacked = false;
    // }

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
