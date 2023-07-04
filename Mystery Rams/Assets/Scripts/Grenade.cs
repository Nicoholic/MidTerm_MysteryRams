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
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

    foreach (Collider nearbyObject in colliders) 
        { 
            rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null) 
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            
            }
        
        }
        Destroy(gameObject);
    }
}
