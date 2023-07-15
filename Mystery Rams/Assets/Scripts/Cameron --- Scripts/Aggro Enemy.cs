using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class AggroEnemy : MonoBehaviour, IDamage {

    [Header("Stats")]
    [SerializeField] int HP;

    [SerializeField] GameObject projectile;

    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float spread;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float bulletsPerShot;

    [SerializeField] float attackRange;

    [SerializeField] float shootForce;
    [SerializeField] float upwardForce;

    [SerializeField] Transform attackPoint;

    [Header("Debug")]
    [SerializeField] bool playerInAttackRange;
    [SerializeField] bool alreadyAttacked;
    [SerializeField] int bulletsShot;

    private NavMeshAgent agent;
    private Transform player;

    private LayerMask whatIsGround;
    private LayerMask whatIsPlayer;


    void Start() {
        player = GameManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        whatIsGround = 10;
        whatIsPlayer = LayerMask.GetMask("Player");
        alreadyAttacked = false;
    }

    void Update() {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange) 
            ChasePlayer();
        
        if (playerInAttackRange) 
            AttackPlayer();
    }

    private void ChasePlayer() {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer() {
        agent.SetDestination(transform.position);

        //change this
        transform.LookAt(player.position);

        if (!alreadyAttacked) {
            bulletsShot = 0;
            Shoot();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Shoot() {
        Vector3 directionWithoutSpread = player.position - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.transform.up * upwardForce, ForceMode.Impulse);

        bulletsShot++;

        if (bulletsShot < bulletsPerShot)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage) {
        HP -= damage;

        if (HP <= 0)
            Invoke(nameof(DelayedDestroy), 0.025f);
    }

    private void DelayedDestroy() => Destroy(gameObject);

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}