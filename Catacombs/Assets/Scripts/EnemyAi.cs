
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform player;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPC;

    public LayerMask whatIsGround, whatIsPlayer;

    // public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    // public float timeBetweenAttacks;
    // bool alreadyAttacked;
    // public GameObject projectile;

    //States
    public float sightRange, warningRange; //, attackRange;
    public bool playerInSightRange, playerInWarningRange; //, playerInAttackRange;
    public GameObject hunting, warning;
    public AudioSource chase_audio_source;
    public float chase_volume = 0.0f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        FPC = player.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInWarningRange = Physics.CheckSphere(transform.position, warningRange, whatIsPlayer);
        // playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange || FPC.hiding) {
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
            
        // if (playerInSightRange && !playerInAttackRange) 
        // if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
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
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        // float randomZ = Random.Range(-walkPointRange, walkPointRange);
        // float randomX = Random.Range(-walkPointRange, walkPointRange);

        // walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        //     walkPointSet = true;
        float radius = walkPointRange;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        walkPointSet = true;
        walkPoint = finalPosition;
    }

    private void ChasePlayer()
    {
        hunting.SetActive(true);
        warning.SetActive(false);
        walkPointSet = false;
        // Debug.Log(player.position + "");
        agent.SetDestination(player.position);
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

    // public void TakeDamage(int damage)
    // {
    //     health -= damage;

    //     if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    // }
    // private void DestroyEnemy()
    // {
    //     Destroy(gameObject);
    // }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, warningRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
