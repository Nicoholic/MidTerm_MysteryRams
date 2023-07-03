using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject detonationEffect;
    [SerializeField] Rigidbody rb;
    

    [Header("Grenade Stats")]
    float explosionTimer = 2f;
    float explosionCountdown;
    float explosionRadius = 3f;
    float explosionForce = 600f;

    bool hasExploded;
    // Start is called before the first frame update
    void Start()
    {
        explosionCountdown = explosionTimer;
    }

    // Update is called once per frame
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
