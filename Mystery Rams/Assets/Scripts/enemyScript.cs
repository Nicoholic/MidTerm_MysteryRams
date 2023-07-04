using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class enemyScript : MonoBehaviour, IDamage//damage script implementation should be IDamage. Doesn't seem to work properly.
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking 
    //public float timeBetweenAttacks;
    bool isAttacking;

    

    [Header("Enemy Info")]
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    //States 
    [SerializeField] public float sightRange, attackRange;
    [SerializeField] public bool playerInSightRange, playerInAttackRange;
    //Functions that haven't been added:
    //Roaming functionality (to be done with AI)
    //View Angle (to be done with movement)
    //Rotation Speed (to be done with movement)

    [Header ("Weaponry Info")]
    [SerializeField] GameObject bullet;
    [SerializeField] float firerate;
    [SerializeField] Transform bulletspawn;


    //Vector3 initSpawn;
    void Start()
    {
        //Code would go here to change objective count if we decide to count enemies.
        //initSpawn = transform.position
        //find the player 
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();


    }


    void Update()
    {
        //the sightrange for the AI 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //attack range 
        playerInAttackRange = Physics.CheckSphere(transform.position,attackRange, whatIsPlayer);

        // if player is out of sight and attack range roam
        if (!playerInSightRange && !playerInAttackRange)
        {
            //StartCoroutine("Patrol");
            Patroling();
            
        }
        // if player is in sight but out of range chase 
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        // if player is in sight and in range attack 
        if(playerInAttackRange && playerInSightRange) AttackPlayer();
        
    }

    public void TakeDamage(int damage) //Needs the damage script to work.
    {
        HP -= damage;
        //Something something code here to move toward player when we implement AI
        StartCoroutine(damageindicator());

        if (HP <= 0)
        {
            //Code would go here to change objective count if we decide to count enemies.
            Destroy(gameObject);
        }

        
        

    }
    IEnumerator damageindicator()
    {
        for (int i = 0; i < 3; i++)
        {
            model.material.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            model.material.color = Color.white;
        }
        
    }


    private void Patroling()
    {
        //if walkpoint is not set search for a new point 
        if (!walkPointSet) SearchWalkPoint();
        //when point is found set destination 
        if(walkPointSet)
            agent.SetDestination(walkPoint);
        //seeing if i reached my destination 
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walk point reached 
        if (distanceToWalkPoint.magnitude < 5f)
        {
            
            walkPointSet = false;

        }

    }
    private void SearchWalkPoint()
    {
        //calculating random walk point in range 
        float walkPointZ = Random.Range(-walkPointRange, walkPointRange);
        float walkPointX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + walkPointX, 0, transform.position.z + walkPointZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

        

    }

    
    private void ChasePlayer()
    {
        //need to add the stop dist 
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
       //make sure enemy doesnt move 
       agent.SetDestination(transform.position);
       
        // to look at player when attacking 
        transform.LookAt(player);
        
        // Type of attack goes here 
        if(!isAttacking)
        {
            Instantiate(bullet, bulletspawn.position, transform.rotation);
            isAttacking = true;
            Invoke(nameof(ResetAttack), firerate);
        }
    }

    private void ResetAttack()
    {
        isAttacking= false;
    }

    
}
