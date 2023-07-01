using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody rb;

    [Header("Bullet Stats")]
    [SerializeField] int bulletDamage;
    [SerializeField] int bulletSpeed;
    [SerializeField] int destroyBulletTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyBulletTime);
        rb.velocity = transform.forward * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageableComponent = other.GetComponent<IDamage>();
        if (damageableComponent != null ) 
        {
            damageableComponent.DamageTaken(bulletDamage);
        
        }

        Destroy(gameObject);
    }
}
