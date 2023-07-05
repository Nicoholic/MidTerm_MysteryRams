using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Keybinds & Settings")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] public float dashSpeed;


    [SerializeField] float speedIncreaseMultiplier;
    [SerializeField] float slopeIncreaseMultiplier;

    public float maxYSpeed;

    [SerializeField] float groundDrag;

    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private float horizontalInput;
    private float verticalInput;

    Vector3 moveDirection;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [Range(0, 2)][SerializeField] float airMultiplier;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
    [SerializeField] float momentumPenalty;

    private float yScaleOriginal;

    [Header("Sliding")]
    [SerializeField] float maxSlideTime;
    [SerializeField] float slideForce;
    [Range(0, 1)][SerializeField] float slideSensitivity;
    [SerializeField] bool doSensLerp;

    private float originalSensitivity;
    private float slideTimer;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;

    [Header("Components")]
    [SerializeField] Transform orientation;
    [SerializeField] PlayerCamera playerCamera;

    private Rigidbody rb;

    [Header("Debug (READ ONLY)")]
    [SerializeField] float currentSpeed;
    [SerializeField] MovementState state;
    [SerializeField] bool jumpAvailable;
    [SerializeField] bool grounded;
    [SerializeField] bool crouched;

    public bool dashing;
    bool sliding;

    public enum MovementState {
        walking,
        sprinting,
        dashing,
        crouching,
        sliding,
        air
    }


    private void Start() {

        rb = GetComponent<Rigidbody>();
        playerCamera = GameManager.instance.playerCamera.GetComponent<PlayerCamera>();
        orientation = GameManager.instance.player.transform.GetChild(0);

        rb.freezeRotation = true;
        jumpAvailable = true;

        yScaleOriginal = transform.localScale.y;
        originalSensitivity = playerCamera.GetSensitivity();

        Invoke(nameof(GameManager.instance.SpawnPlayer), 0.0025f);
    }

    private void Update() {

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded && state != MovementState.dashing)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

    }

    private void FixedUpdate() {
        MovePlayer();
        if (sliding)
            SlidingMovement();
    }

    /// <summary>
    /// Handles what state of movement the player is in
    /// </summary>
    private void StateHandler() {

        // Mode - Dashing
        if (dashing) {
            state = MovementState.dashing;
            moveSpeed = dashSpeed;
        }

        // Mode - Sliding
        else if (sliding) {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Crouching
        else if (Input.GetKey(crouchKey)) {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        //// Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey)) {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded) {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else {
            state = MovementState.air;
        }

        // check if desiredMoveSpeed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0) {
            StopAllCoroutines();
            StartCoroutine(LerpMoveSpeed());
        } else {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    /// <summary>
    /// Smoothly changes the move speed of the player to the cap if they go over, allowing momentum from high speeds to be held for a bit
    /// </summary>
    private IEnumerator LerpMoveSpeed() {

        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference) {
            if (state == MovementState.crouching)
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, momentumPenalty * (time / difference));
            else
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);


            if (OnSlope()) {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            } else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    /// <summary>
    /// Input Manager for the player
    /// </summary>
    private void MyInput() {

        if (!sliding) {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }


        if (Input.GetKey(jumpKey) && jumpAvailable && grounded) {

            jumpAvailable = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

            if (sliding)
                StopSlide();

        }

        if (Input.GetKeyDown(crouchKey)) {

            if (!crouched) {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                crouched = true;
            }

            if (((horizontalInput != 0 || verticalInput != 0) && grounded && !sliding) || OnSlope() && !sliding) {
                StartSlide();
            }
        }

        if (Input.GetKeyUp(crouchKey) && state != MovementState.sliding) {
            transform.localScale = new Vector3(transform.localScale.x, yScaleOriginal, transform.localScale.z);
            crouched = false;
        }
    }

    private void MovePlayer() {

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope) {
            rb.AddForce(20f * moveSpeed * GetSlopeMoveDirection(moveDirection), ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);

        } else if (grounded)
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);

        rb.useGravity = !OnSlope();
        currentSpeed = moveDirection.magnitude * moveSpeed;
    }

    /// <summary>
    /// Limits the velocity of the character if speed goes over cap
    /// </summary>
    private void SpeedControl() {

        if (OnSlope() && !exitingSlope) {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        } else {
            Vector3 flatVelocity = new(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVelocity.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.y);
    }

    private void Jump() {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        jumpAvailable = true;
        exitingSlope = false;
    }

    private bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction) {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    /// <summary>
    /// Shrinks player into crouched state and starts slide
    /// </summary>
    private void StartSlide() {

        sliding = true;
        if (!crouched) {
            crouched = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (doSensLerp)
            playerCamera.LerpSensitivity(originalSensitivity * slideSensitivity);
        slideTimer = maxSlideTime;
    }

    /// <summary>
    /// Applies force to player based on slope angle and stops if slide timer is <= 0
    /// </summary
    private void SlidingMovement() {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (!OnSlope() || rb.velocity.y > -0.1f) {

            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        } else {
            rb.AddForce(GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide() {
        sliding = false;
        if (crouched && !Input.GetKey(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, yScaleOriginal, transform.localScale.z);
            crouched = false;
        }

        if (doSensLerp)
            playerCamera.LerpSensitivity(originalSensitivity);
    }

    private void SpawnPlayer() {
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        GameManager.instance.SpawnPlayer();
    }
}
