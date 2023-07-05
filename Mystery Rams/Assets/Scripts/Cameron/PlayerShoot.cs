using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IDamage {

    [Header("Player Stats")]
    [SerializeField] public int HP;
    public int maxHP;

    [Header("Gun Stats")]
    [SerializeField] float rate;
    [SerializeField] int damage;
    [SerializeField] int range;

    [SerializeField] bool doDebug;

    public Camera playerCamera;

    bool isShooting;

    void Start() {
        maxHP = HP;
        playerCamera = GameManager.instance.playerCamera.GetComponent<Camera>();
    }

    void Update() {
        if (Input.GetMouseButton(0) && !isShooting)
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot() {
        isShooting = true;


        if (Physics.Raycast(playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, range)) {
            if (doDebug)
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            if (hit.collider.TryGetComponent<IDamage>(out var damageable) && !hit.collider.TryGetComponent<PlayerMovement>( out _) )
                damageable.TakeDamage(damage);
        } else if (doDebug) {
            Debug.Log("Raycast missed.");
        }

        yield return new WaitForSeconds(rate);
        isShooting = false;
    }

    public void TakeDamage(int damage) {
        HP -= damage;
        if (HP <= 0) {
            GameManager.instance.GameLoss();
        }
    }
}
