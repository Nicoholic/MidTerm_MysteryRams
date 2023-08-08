using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour {
    [SerializeField] GameObject[] enemyList;
    [SerializeField] GameObject[] roomTriggers;

    public int enemiesAlive;
    void Awake() {
        foreach (var enemy in enemyList)
            enemy.SetActive(false);
    }

    public void ActivateRoom() {
        foreach (var enemy in enemyList) {
            enemy.SetActive(true);
            enemiesAlive++;
        }

        foreach (var trigger in roomTriggers)
            trigger.SetActive(false);
    }

    private void DeactivateRoom() {

    }



    public void CheckEnemiesAlive() {
        if (enemiesAlive <= 0)
            foreach (var trigger in roomTriggers)
                Destroy(trigger);
    }
}
