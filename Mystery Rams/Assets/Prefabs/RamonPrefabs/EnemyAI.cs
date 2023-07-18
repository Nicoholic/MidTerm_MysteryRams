using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    //Apologies for the wait. This is the new enemy script.
    [Header("Components")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headpos;
    [SerializeField] Image HPBar;
    [SerializeField] Animator anim;

    [Header("Stats")]
    [SerializeField] int HP;
    [SerializeField] int shootAngle;
    [SerializeField] float viewAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int roamtime;
    [SerializeField] int roamdist;

    [Header("Weaponry")]
    [SerializeField] float ShotRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform ShootPos;

    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    bool isShooting;
    float stoppingDistanceOrig;
    Vector3 startingPos;
    bool destinationChosen;
    int hporig;
    bool isdead;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.UpdateGameGoal(1);
        stoppingDistanceOrig = agent.stoppingDistance;
        hporig = HP;
        startingPos = transform.position;
        isdead = false;
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Blend", agent.velocity.normalized.magnitude);

            if (playerInRange && !canSeePlayer())
            {
                StartCoroutine(roaming());
            }
            else if (agent.destination != GameManager.instance.player.transform.position)
            {
                StartCoroutine(roaming());
            }
        }

            
    }

    IEnumerator roaming()
    {
        if(agent.remainingDistance < 0.05 && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamtime);

            Vector3 randomPos = Random.insideUnitSphere * roamdist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamdist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        Instantiate(bullet, ShootPos.position, transform.rotation);
        yield return new WaitForSeconds(ShotRate);
        isShooting = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistanceOrig;
        playerDir = GameManager.instance.player.transform.position - headpos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headpos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            {

                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }


        }
        agent.stoppingDistance = 0;
        return false;
    }

    void IDamage.TakeDamage(int amount)
    {
        HP -= amount;
        updateUI();
        

        if (HP <= 0 && !isdead)
        {
            StopAllCoroutines();
            isdead = true;
            GameManager.instance.UpdateGameGoal(-1);
            anim.SetBool("Death", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            //Destroy(gameObject);
        }
        else
        {
            anim.SetTrigger("Damage");
            agent.SetDestination(GameManager.instance.player.transform.position);
            StartCoroutine(flashdmg());
        }
    }

    IEnumerator flashdmg()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
    public void updateUI()
    {
        HPBar.fillAmount = (float)HP / hporig;
    }
}
