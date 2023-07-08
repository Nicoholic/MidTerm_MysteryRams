using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject detonationEffect;
    [SerializeField] Rigidbody rb;


    [Header("Grenade Stats")]
    [SerializeField] float explosionTimer;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;
    [SerializeField] float explosionCountdown;
    [SerializeField] int explosionDamage;
 

    bool hasExploded;
    
    void Start()
    {
        
        explosionCountdown = explosionTimer;
    }

    
    void Update()
    {
        explosionCountdown -= Time.deltaTime;
        if (explosionCountdown <= 0f && !hasExploded) 
        {
        Explode();
            hasExploded = true;
        }

    }
    void Explode() 
    {
    Instantiate(detonationEffect, transform.position,transform.rotation);
      
        
    }

   


}
