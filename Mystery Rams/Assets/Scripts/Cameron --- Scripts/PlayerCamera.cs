using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour {

    [Header("Components")]
    [SerializeField] private Transform orientation;

    [Header("Sensitivity")]
    [SerializeField] public float sensitivity = 100f;
    [SerializeField] private Slider slider;

    private float xRotation = 0f;
    private float yRotation;

    float mouseX;
    float mouseY;

    void Start() {
        slider = GameManager.instance.senSlider;
        orientation = GameManager.instance.player.transform.GetChild(0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensitivity = PlayerPrefs.GetFloat("currentSensitivity", 100);
        slider.value = (sensitivity - 10) / 90;
        slider.onValueChanged.AddListener(delegate {ChangeSpeed(slider.value); });
    }


    void Update() {

        PlayerPrefs.SetFloat("currentSensitivity", sensitivity);
        if (!GameManager.instance.isPaused) {
            mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;


            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);


            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void ChangeSpeed(float newspeed) 
    {
        sensitivity = newspeed * 90 + 10;
    }    

    public IEnumerator LerpFov(float endValue, float duration = 0.1f) {
        float startValue = GetComponent<Camera>().fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            float currentFov = Mathf.Lerp(startValue, endValue, t);
            GetComponent<Camera>().fieldOfView = currentFov;
            gameObject.transform.GetChild(0).GetComponent<Camera>().fieldOfView = currentFov;


            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GetComponent<Camera>().fieldOfView = endValue;
        gameObject.transform.GetChild(0).GetComponent<Camera>().fieldOfView = endValue;
    }

    public float GetSensitivity() {
        return sensitivity;
    }

    public void LerpSensitivity(float endValue) {
        StartCoroutine(ChangeSensitivityOverTime(endValue));
    }

    private IEnumerator ChangeSensitivityOverTime(float endValue) {
        float startValue = sensitivity;
        float elapsedTime = 0f;
        float duration = 0.2f;

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
