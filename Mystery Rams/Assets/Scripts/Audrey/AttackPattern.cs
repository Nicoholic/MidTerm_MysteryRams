using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    [SerializeField] float attackDelay;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float spread;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float bulletsPerShot;

    [SerializeField] float attackRange;

    [SerializeField] float shootForce;
    [SerializeField] float upwardForce;
}
