using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BouncePad : MonoBehaviour {

    [SerializeField] float jumpForce;
    private void OnTriggerEnter(Collider other) {

        if (other.TryGetComponent<Rigidbody>(out var rb))
            rb.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

    }
}
