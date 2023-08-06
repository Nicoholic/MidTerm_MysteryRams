using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    //Apologies for the wait. This is the new enemy script.
    [Header("Components")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headpos;
    //[SerializeField] Image HPBar;
    [SerializeField] Animator anim;

    [Header("Stats")]
    [SerializeField] int HP;
    [SerializeField] int shootAngle;
    [SerializeField] float viewAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int roamtime;
    [SerializeField] int roamdist;
    [SerializeField] float Range;

    [Header("Weaponry")]
    [SerializeField] bool isRigid;
    [SerializeField] float ShotRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform ShootPos;
    [SerializeField] float bulletSpeed;

    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    bool isShooting;
    float stoppingDistanceOrig;
    Vector3 startingPos;
    bool destinationChosen;
    int hporig;
    bool ishurt;
    bool isdead;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.UpdateGameGoal(1);
        stoppingDistanceOrig = agent.stoppingDistance;
        hporig = HP;
        startingPos = transform.position;
        isdead = false;
        //updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Blend", agent.velocity.normalized.magnitude);
            playerInRange = Physics.CheckSphere(transform.position, Range, LayerMask.GetMask("Player"));

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

    /*void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }*/

    IEnumerator shoot()
    {
        if (!isRigid)
        {
            isShooting = true;
            anim.SetTrigger("Shoot");
            Instantiate(bullet, ShootPos.position, transform.rotation);
            yield return new WaitForSeconds(ShotRate);
            isShooting = false;
        }
        else
        {
            isShooting = true;
            direction = GameManager.instance.player.transform.position - ShootPos.position;
            GameObject currentBullet = Instantiate(bullet, ShootPos.position, Quaternion.identity);
            currentBullet.transform.forward = direction.normalized;
            currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
            yield return new WaitForSeconds(ShotRate);
            isShooting = false;
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }*/

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

    //void IDamage.TakeDamage
    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(GameManager.instance.Hitmark());
        //updateUI();
        

        if (HP <= 0)
        {
            /*if (!isdead)
            {
                GameManager.instance.UpdateGameGoal(-1);
            }*/
            isdead = true;
            StopAllCoroutines();
            GameManager.instance.hitmarker.SetActive(false);
            model.material.color = Color.white;
            anim.SetBool("Death", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            Invoke("Kill", 5f);
        }
        else if (!isdead)
        {
            if (!ishurt)
            {
                anim.SetTrigger("Damage");
                ishurt = true;
            }
            agent.SetDestination(GameManager.instance.player.transform.position);
            StartCoroutine(playHurtAnim());
            StartCoroutine(flashdmg());
            
        }
        
    }

    IEnumerator flashdmg()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
    /*public void updateUI()
    {
        HPBar.fillAmount = (float)HP / hporig;
    }*/
    IEnumerator playHurtAnim()
    {
        yield return new WaitForSeconds(0.5f);
        ishurt = false;
    }
    private void Kill()
    {
        Destroy(gameObject);
    }
}
