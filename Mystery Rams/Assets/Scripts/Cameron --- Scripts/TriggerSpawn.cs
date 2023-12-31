using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class TriggerSpawn : MonoBehaviour {
    public GameObject[] enemyList;
    [SerializeField] SlidingDoor[] doorList;
    public int enemyCount;
    void Awake() {
        foreach (var enemy in enemyList) {
            if (enemy != null) {
                enemy.SetActive(false);
                enemyCount++;
            }
        }
    }

    private void FixedUpdate() {
        if (enemyCount <= 0)
            foreach (var door in doorList) {
                if (door != null)
                    door.UnlockDoor();
            }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            foreach (var enemy in enemyList) {
                if (enemy != null) {
                    enemy.SetActive(true);
                    if (enemy.TryGetComponent<AggroEnemy>(out var aggroEnemy))
                        aggroEnemy.spawner = this;
                }
            }

            foreach (var door in doorList)
                if (door != null && enemyCount > 0)
                    door.LockDoor();
        }
    }
}
