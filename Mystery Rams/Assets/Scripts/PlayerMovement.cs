using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Components")]
    public Transform orientation;

    [Header("Movement")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float groundDrag;

    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldown;
    [Range(0,2)][SerializeField] public float airMultiplier;
    bool canJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start() {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
    }

    private void Update() {

        //Checks if the player is on the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        //If player is on the ground apply drag
        if (grounded)
            rb.drag = groundDrag;
        else 
            rb.drag = 0;
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MyInput() {

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetButton("Jump") && canJump && grounded) {

            canJump = false;

            Jump();

            //Calls the method after the jumpCooldown
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer() {

        //This Calculates the movement direction
        moveDirection = (orientation.forward * verticalInput) +
                           (orientation.right * horizontalInput);
 
        if (grounded) {
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

    }

    //Limits the velocity of the character if speed goes over cap
    private void SpeedControl() {

        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVelocity.magnitude > moveSpeed) {

            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump() {

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        canJump = true;
    }
}
