using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class TriggerSpawn : MonoBehaviour {
    [SerializeField] GameObject[] enemyList;
    void Awake() {
        foreach (var enemy in enemyList)
            enemy.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            foreach (var enemy in enemyList)
                enemy.SetActive(true);
            Destroy(gameObject);
        }
    }
}
