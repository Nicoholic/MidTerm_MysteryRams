using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BouncePad : MonoBehaviour {

    [SerializeField] float jumpForce;
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            other.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }
}
