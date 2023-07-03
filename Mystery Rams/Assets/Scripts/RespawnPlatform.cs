using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour {

    public GameObject spawnPos;

    private void OnTriggerEnter(Collider other) {
        GameManager.instance.playerSpawnPos = spawnPos;
    }
}
