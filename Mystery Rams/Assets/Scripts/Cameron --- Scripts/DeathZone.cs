using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] bool doDeath;
    [SerializeField] bool doDamage;
    [SerializeField] int damage;
    [SerializeField] bool doDestroyGameObject;
    [SerializeField] bool doRespawn;

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.CompareTag("Player")) {

            if (doDeath) {
                GameManager.instance.GameLoss();
                return;

            } else if (doDamage)
                other.GetComponent<IDamage>().TakeDamage(damage);

            if (other.GetComponent<PlayerMovement>().HP > 0 && doRespawn)
                GameManager.instance.player.GetComponent<PlayerMovement>().SpawnPlayer();


        } else if (doDestroyGameObject)
            Destroy(other.gameObject);
    }
}
