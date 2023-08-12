using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackPattern : ScriptableObject
{
   public GameObject projectile;
   
   public float attackDelay;
   public float timeBetweenAttacks;
   public float spread;
   public float timeBetweenShots;
   public float bulletsPerShot;
    
   public float attackRange;
   
   public float shootForce;
   public float upwardForce;
}
