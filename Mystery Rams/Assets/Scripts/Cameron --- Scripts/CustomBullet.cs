using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class CustomBullet : MonoBehaviour {

    [Header("Components")]
    public Rigidbody rb;
    public GameObject explosion;
    public string targetTag;

    [Header("Stats")]
    public float bounciness;
    public bool useGravity;

    [Header("Damage")]
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    [Header("Lifetime")]
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    bool exploded;

    int collisions;
    PhysicMaterial physicalMaterial;

    private void Start() {
        Setup();
    }

    private void Setup() {
        physicalMaterial = new PhysicMaterial {
            bounciness = bounciness,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounceCombine = PhysicMaterialCombine.Minimum
        };

       // GetComponent<SphereCollider>().material = physicalMaterial;

        rb.useGravity = useGravity;
    }

    private void FixedUpdate() {
        if (collisions > maxCollisions)
            Explode();

        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0 && !exploded)
            Explode();
    }

    private void Explode() {
        exploded = true;

        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (var item in enemies) {

            if (item.TryGetComponent<Rigidbody>(out var itemRB))
                itemRB.AddExplosionForce(explosionForce, transform.position, explosionRange);

            if (item.TryGetComponent<IDamage>(out var enemy) && item.CompareTag(targetTag))
                enemy.TakeDamage(explosionDamage);
        }

        Invoke(nameof(DelayedDestroy), 0.005f);
    }

    private void DelayedDestroy() => Destroy(gameObject);

    private void OnCollisionEnter(Collision collision) {
        collisions++;
        if (collision.collider.CompareTag(targetTag) && explodeOnTouch)
            Explode();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
