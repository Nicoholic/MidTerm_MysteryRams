using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrigger : MonoBehaviour {
    [SerializeField] DungeonSpawner parent;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            parent.ActivateRoom();
    }
}
