using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    [Header("Components")]
    [SerializeField] private Transform orientation;

    [Header("Sensitivity")]
    [SerializeField] public float xSensitivity;
    [SerializeField] private float ySensitivity;

    private float xRotation;
    private float yRotation;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {

        float mouseX = Input.GetAxis("Mouse X") * xSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * ySensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue) {
        GetComponent<Camera>().fieldOfView = endValue;
    }
}
