using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    [Header("Components")]
    [SerializeField] private Transform orientation;

    [Header("Sensitivity")]
    [SerializeField] public float sensitivity;

    private float xRotation;
    private float yRotation;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public IEnumerator LerpFov(float endValue, float duration = 0.1f) {
        float startValue = GetComponent<Camera>().fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            float currentFov = Mathf.Lerp(startValue, endValue, t);
            GetComponent<Camera>().fieldOfView = currentFov;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GetComponent<Camera>().fieldOfView = endValue;
    }

    public float GetSensitivity() {
        return sensitivity;
    }

    public IEnumerator LerpSensitivity(float endValue, float duration = 0.1f) {
        float startValue = sensitivity;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            float currentSensitivity = Mathf.Lerp(startValue, endValue, t);
            sensitivity = currentSensitivity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sensitivity = endValue;
    }
}
