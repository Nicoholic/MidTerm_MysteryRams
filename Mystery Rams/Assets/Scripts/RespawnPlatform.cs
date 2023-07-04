using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour {

    public GameObject spawnPos;
    public Renderer inside;
    public Collider trigger;

    Color colorActive = new(0.4f, 0.9f, 0.7f, 1.0f);
    Color colorInactive = new(0.0f, 0.0f, 0.0f, 1.0f);

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            GameManager.instance.UpdateSpawnPlatforms();
            GameManager.instance.playerSpawnPoint = spawnPos;
            inside.material.color = colorActive;
        }
    }

    private void Start() {
        SetInactive();
        if (GameManager.instance.playerSpawnPoint.transform.position == spawnPos.transform.position)
            inside.material.color = colorActive;

    }

    /// <summary>
    /// changes the color of the platform
    /// </summary>
    public void SetInactive() {
        inside.material.color = colorInactive;
    }
}
