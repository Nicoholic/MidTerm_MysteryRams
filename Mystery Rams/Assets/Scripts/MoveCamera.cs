using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform CameraPosition;

    private void Start() {
        CameraPosition = GameManager.instance.player.transform.GetChild(1);
    }
    void Update() {
        transform.position = CameraPosition.position;
    }
}
