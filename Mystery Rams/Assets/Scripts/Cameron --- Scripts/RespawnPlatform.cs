using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour {

    public GameObject respawnPoint;
    public Renderer inside;

    Color colorActive = new(0.4f, 0.9f, 0.7f, 1.0f);
    Color colorInactive = new(0.0f, 0.0f, 0.0f, 1.0f);

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            UpdateRespawnPoints();

            GameManager.instance.playerSpawnPoint = respawnPoint;
            inside.material.color = colorActive;
        }
    }

    private void UpdateRespawnPoints() {
        RespawnPlatform[] respawnPlatforms = Resources.FindObjectsOfTypeAll<RespawnPlatform>();
        foreach (var respawnPlatform in respawnPlatforms) {
            respawnPlatform.inside.sharedMaterial.color = colorInactive;
        }
    }
}
