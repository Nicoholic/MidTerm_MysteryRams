using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            GameManager.instance.GameLoss();
        else
            Destroy(other.gameObject);
    }
}
