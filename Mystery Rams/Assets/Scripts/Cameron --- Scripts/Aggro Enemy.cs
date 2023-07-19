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

    [SerializeField] float attackDelay;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float spread;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float bulletsPerShot;

    [SerializeField] float attackRange;

    [SerializeField] float shootForce;
    [SerializeField] float upwardForce;

    [SerializeField] Transform attackPoint;

    [SerializeField] Animator animator;

    [Header("Debug")]
    [SerializeField] bool playerInAttackRange;
    [SerializeField] bool canSeePlayer;
    [SerializeField] bool attacking;
    [SerializeField] int bulletsShot;

    [SerializeField] Renderer model;

    private NavMeshAgent agent;
    private Transform player;

    private LayerMask whatIsGround;
    private LayerMask whatIsPlayer;


    void Start() {
        player = GameManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        whatIsGround = 10;
        whatIsPlayer = LayerMask.GetMask("Player");
        attacking = false;
    }

    void FixedUpdate() {
        if (animator != null && animator.gameObject.activeSelf)
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        canSeePlayer = Physics.Raycast(attackPoint.position, player.position - attackPoint.position, out var hit) && hit.collider.CompareTag("Player");

        if (playerInAttackRange && canSeePlayer) {
            if (animator != null)
                animator.SetTrigger("Attack");
            Invoke(nameof(AttackPlayer), attackDelay);
        } else if (!attacking)
            ChasePlayer();
    }

    private void ChasePlayer() {
        if (HP > 0)
            agent.SetDestination(player.position);
    }

    private void AttackPlayer() {
        if (HP > 0)
            agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.position.x, 0, player.position.z));

        if (!attacking) {
            bulletsShot = 0;



            Shoot();

            attacking = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Shoot() {
        Vector3 directionWithoutSpread = player.position - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, z);

        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.transform.up * upwardForce, ForceMode.Impulse);

        bulletsShot++;

        if (bulletsShot < bulletsPerShot)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    private void ResetAttack() {
        attacking = false;
    }

    IEnumerator FlashDamage() {
        if (model != null) {
            model.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            model.material.color = Color.white;
        }

    }

    private void DelayedDestroy() => Destroy(gameObject);

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(int damage) {

        HP -= damage;
        StartCoroutine(FlashDamage());
        StartCoroutine(GameManager.instance.Hitmark());


        if (HP <= 0) {
            Invoke(nameof(DelayedDestroy), 0.025f);
            Invoke(nameof(DelayRemoveHit), 0.02f);
        }
    }
          
    private void DelayRemoveHit() => GameManager.instance.hitmarker.SetActive(false);
}