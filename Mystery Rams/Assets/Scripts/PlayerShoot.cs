using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IDamage {

    [Header("Player Stats")]
    [SerializeField] int HP;
    private int maxHP;

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
        maxHP = HP;
    }

    void Update() {
        if (Input.GetKeyDown(shoot) && !isShooting)
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot() {
        isShooting = true;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            Transform objectHit = hit.transform;
            Debug.Log("pew");

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

    public void TakeDamage(int damage) {
        HP = -damage;
        if (HP <= 0)
            Debug.Log("PlayerShoot - Player should be dead now");
    }
}
