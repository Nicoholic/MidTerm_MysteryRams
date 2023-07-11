using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyTest : MonoBehaviour, IDamage {

    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;


    [Header("Enemy Stats")]
    [SerializeField] int hp;
    [Range(1.0f,360.0f)][SerializeField] private float viewAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int roamTimer;
    [SerializeField] int roamDistance;

    [Header("Gun Stats")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float inaccuracy;

    public bool doDebug;
    public bool shotgun;
    public int gunPellets;

    bool playerInRange;
    bool isShooting;
    bool destinationChosen;
    float originalStoppingDistance;
    float angleToPlayer;
    Vector3 playerDirection;
    Vector3 startingPos;
    bool isdead;
   

    void Start() {
        GameManager.instance.UpdateGameGoal(1);
        originalStoppingDistance = agent.stoppingDistance;
        startingPos = transform.position;
        isdead = false;
    }

    void Update() {
        if (playerInRange && !CanSeePlayer()) {
            StartCoroutine(Roam());
        } else if (agent.destination != GameManager.instance.player.GetComponent<Rigidbody>().transform.position)
            StartCoroutine(Roam());
    }

    IEnumerator Roam() {
        if (agent.remainingDistance < 0.05f && !destinationChosen) {

            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);

            Vector3 randomPos = Random.insideUnitCircle * roamDistance;
            randomPos += startingPos;

            NavMesh.SamplePosition(randomPos, out NavMeshHit hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    void FacePlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);

    }

    bool CanSeePlayer() {

        agent.stoppingDistance = originalStoppingDistance;
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        if (doDebug) {
            Debug.Log(angleToPlayer + "     " + viewAngle);
            Debug.DrawRay(headPos.position, playerDirection);
        }

        if (Physics.Raycast(headPos.position, playerDirection, out RaycastHit hit)) {

            if (doDebug)
                Debug.Log($"{hit.collider.CompareTag("Player")}    {angleToPlayer < viewAngle}");

            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle) {

                if (doDebug)
                    Debug.Log("EnemyTest - Can see player");

                agent.SetDestination(GameManager.instance.player.GetComponent<Rigidbody>().transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance) {
                    FacePlayer();

                    if (!isShooting) {
                        StartCoroutine(Shoot());
                    }
                    return true;
                }
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    IEnumerator Shoot() {
        isShooting = true;
        
        if (shotgun) {
            for (int i = 0; i < 15; i++) {
                Quaternion randomRotationOffset = Quaternion.Euler( Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy));
                Instantiate(bullet, shootPos.position, transform.rotation * randomRotationOffset);
            }
        } else
            Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
        }
    }

    void IDamage.TakeDamage(int amount) {

        if (doDebug)
            Debug.Log("EnemyTest - Took damage: " + amount);

        hp -= amount;
       
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(FlashDamage());

        if (hp <= 0) {
            
            if (isdead==false)
            {
                isdead = true;
                GameManager.instance.UpdateGameGoal(-1);
            }
            Destroy(gameObject);
        }
    }
    IEnumerator FlashDamage() {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
