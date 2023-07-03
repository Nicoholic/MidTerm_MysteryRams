using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
            GameManager.instance.SpawnPlayer();
        else
            Destroy(other.gameObject);
    }
}
