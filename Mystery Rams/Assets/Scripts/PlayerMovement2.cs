using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Components")]
    public Transform orientation;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode slideKey = KeyCode.LeftControl;

    [Header("Movement")]
    public float walkSpeed;
    public float slideSpeed;
    public float airMultiplier;
    public float groundDrag;
    private float moveSpeed;
    private float desiredMoveSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    private bool readyToJump;

    [Header("Sliding")]
    public float slideYScale;
    private float startYScale;
    private Vector3 slideDirection;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    private Vector3 moveDirection;
    private Rigidbody rb;

    public MovementState state;
    public enum MovementState {
        walking,
        sliding,
        air
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate() {
        if (state != MovementState.sliding) {
            MovePlayer();
        } else {
            SlidingMovement();
        }
    }

    private void MyInput() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize();

        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
            if (state == MovementState.sliding) {
                StopSlide();
            }
        }

        if (Input.GetKeyDown(slideKey)) {
            StartSlide();
            slideDirection = orientation.forward;
        }

        if (Input.GetKeyUp(slideKey)) {
            StopSlide();
        }
    }

    private void StateHandler() {
        if (state == MovementState.sliding) {
            desiredMoveSpeed = slideSpeed;
        } else if (grounded) {
            desiredMoveSpeed = walkSpeed;
        } else {
            desiredMoveSpeed = walkSpeed * airMultiplier;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer() {
        if (grounded) {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        } else if (!grounded) {
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl() {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }

    private void StartSlide() {
        state = MovementState.sliding;
        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    private void SlidingMovement() {

        if (Input.GetKey(jumpKey)) {
            StopSlide();
            return;
        }
        rb.velocity = slideDirection * slideSpeed;
    }

    private void StopSlide() {
        state = MovementState.walking;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }
}



