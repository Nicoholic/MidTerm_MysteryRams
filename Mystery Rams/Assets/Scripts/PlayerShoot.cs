using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    [Header("Gun Stats")]
    [SerializeField] float rate;
    [SerializeField] int damage;
    [SerializeField] int range;

    [Header("Components")]
    [SerializeField] Camera playerCamera;

    [Header("Keybinds")]
    [SerializeField] KeyCode shoot = KeyCode.Mouse0;

    bool isShooting;

    public Component lastThingHit;

    void Start() {

    }

    void Update() {
        if (Input.GetKeyDown(shoot) && !isShooting)
            Shoot();
    }

    private void FixedUpdate() {

        
    }

    IEnumerator Shoot() {
        isShooting = true;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            Transform objectHit = hit.transform;
            if (hit.collider.TryGetComponent<IDamage>(out var damageable)) {
                damageable.TakeDamage(damage);

                //debug
                lastThingHit = damageable as Component;
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            }
        }

            yield return new WaitForSeconds(rate);
        isShooting = false;
    }
}
