using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour, IDamage
{

    [Header("Stats")]
    [SerializeField] int HP;
    [SerializeField] AttackPattern[] patterns;
    [SerializeField] Material[] materials;

    [Header("Components")]
    [SerializeField] Transform attackPoint;
    [SerializeField] Animator animator;
    [SerializeField] Renderer model;

    [Header("Current Attack Patterns")]
    [SerializeField] GameObject projectile;
    [SerializeField] float attackDelay;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float spread;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float bulletsPerShot;
    [SerializeField] float attackRange;
    [SerializeField] float shootForce;
    [SerializeField] float upwardForce;


    [Header("Debug")]
    [SerializeField] int currentPhase;
    [SerializeField] bool playerInAttackRange;
    [SerializeField] bool canSeePlayer;
    [SerializeField] bool attacking;
    [SerializeField] int bulletsShot;

    [Header("Sounds")]
    [SerializeField] private AudioSource AttackSound;
    [SerializeField] private AudioSource HurtSound;
    [SerializeField] public GameObject DeathSound;

    public TriggerSpawn spawner;


    private NavMeshAgent agent;
    private Transform player;

    private LayerMask whatIsGround;
    private LayerMask whatIsPlayer;

    private float maxHP;

    public PlayerMovement playerMovement;

    void Start()
    {
        player = GameManager.instance.player.transform;
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        whatIsGround = 10;
        whatIsPlayer = LayerMask.GetMask("Player");
        attacking = false;
        maxHP = HP;
        UpdateAttackPattern(0);
    }

    void FixedUpdate()
    {

        if (animator != null && animator.gameObject.activeSelf)
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        canSeePlayer = Physics.Raycast(attackPoint.position, player.position - attackPoint.position, out var hit) && hit.collider.CompareTag("Player");
        //Renderer rend = hit.transform.GetComponent<Renderer>();
        //rend.material =

        if (playerInAttackRange && canSeePlayer)
        {

            Invoke(nameof(AttackPlayer), attackDelay);
        }
        else if (!attacking)
            ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (HP > 0)
            agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if (HP > 0)
            agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.position.x, 0, player.position.z));

        if (!attacking)
        {
            attacking = true;
            if (AttackSound != null)
                AttackSound.Play();

            bulletsShot = 0;

            if (animator != null)
                animator.SetTrigger("Attack");

            Shoot();


            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Shoot()
    {
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

    private void ResetAttack()
    {
        attacking = false;
    }

    private void DelayedDestroy() => Destroy(gameObject);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(int damage)
    {

        HP -= damage;
        if (HurtSound != null)
            HurtSound.Play();
        StartCoroutine(GameManager.instance.Hitmark());


        if (HP <= 0)
        {
            if (DeathSound != null)
            {
                Instantiate(DeathSound);
            }
            GameManager.instance.GameWin();
            Invoke(nameof(DelayedDestroy), 0.025f);
            Invoke(nameof(DelayRemoveHit), 0.02f);
            playerMovement.BossMusic.Stop();
            playerMovement.BossMusic.loop = false;
            if (spawner != null)
            {
                spawner.enemyCount--;
                if (animator != null)
                    animator.SetTrigger("Dead");
            }

        }
        else
        {
            UpdateAttackPattern((patterns.Count() - Mathf.FloorToInt((HP / maxHP) * patterns.Count())) - 1);
            if (animator != null)
                animator.SetTrigger("Hurt");

        }
    }

    private void DelayRemoveHit() => GameManager.instance.hitmarker.SetActive(false);
    private void UpdateAttackPattern(int newPattern)
    {
        projectile = patterns[newPattern].projectile;

        attackDelay = patterns[newPattern].attackDelay;
        timeBetweenAttacks = patterns[newPattern].timeBetweenAttacks;
        spread = patterns[newPattern].spread;
        timeBetweenShots = patterns[newPattern].timeBetweenShots;
        bulletsPerShot = patterns[newPattern].bulletsPerShot;

        attackRange = patterns[newPattern].attackRange;

        shootForce = patterns[newPattern].shootForce;
        upwardForce = patterns[newPattern].upwardForce;

        currentPhase = newPattern;

        model.material = materials[newPattern];

        if (this.TryGetComponent<MeshRenderer>(out var renderer))
        {
            renderer.material = materials[newPattern];
        }

    }
}
