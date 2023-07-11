using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] bool instantLose;
    [SerializeField] bool doDamage;
    [SerializeField] int damage;
    [SerializeField] bool doDestroy;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (instantLose) {
                GameManager.instance.GameLoss();
                return;
            } else if (doDamage)
                other.GetComponent<IDamage>().TakeDamage(damage);

            if (other.GetComponent<PlayerMovement>().HP > 0)
                GameManager.instance.SpawnPlayer();
        } else if (doDestroy)
            Destroy(other.gameObject);
    }
}
