using UnityEngine;

public class Ydeny : MonoBehaviour
{
    public float targetXRotation = 90f;
    public float targetYRotation = 0f;

    void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;

        Quaternion newRotation = Quaternion.Euler(targetXRotation, targetYRotation, currentRotation.z);

        transform.rotation = newRotation;
    }
}
