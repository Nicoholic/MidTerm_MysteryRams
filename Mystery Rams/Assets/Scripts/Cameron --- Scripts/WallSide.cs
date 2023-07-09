using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSide : MonoBehaviour {
    public float jumpForce = 10f;
    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = 0.25f;

    private float timeToWallUnstick;

    private bool isWalled = false;

    private Vector3 wallJumpDirection;

    private Rigidbody rb;

    void Start() {
        rb = GameManager.instance.player.GetComponent<Rigidbody>();
        timeToWallUnstick = wallStickTime;
    }

    void Update() {
        if (isWalled) {
            if (rb.velocity.y < -wallSlideSpeedMax) {
                rb.velocity = new Vector3(rb.velocity.x, -wallSlideSpeedMax, rb.velocity.z);
            }

            if (timeToWallUnstick > 0) {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetButtonDown("Jump")) {
                    rb.velocity = new Vector3(-Mathf.Sign(Input.GetAxisRaw("Horizontal")) * wallJumpDirection.x, wallJumpDirection.y, wallJumpDirection.z) * jumpForce;
                    isWalled = false;
                }
            } else {
                timeToWallUnstick += Time.deltaTime;
            }
        } else {
            timeToWallUnstick = wallStickTime;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Wall") {
            isWalled = true;
            wallJumpDirection = other.contacts[0].normal;
        }
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Wall") {
            isWalled = false;
        }
    }
}

