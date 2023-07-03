//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour {
//    [Header("Keybinds & Settings")]
//    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
//    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

//    [Header("Movement")]
//    [SerializeField] private float walkSpeed;
//    [SerializeField] private float sprintSpeed;
//    [SerializeField] private float slideSpeed;
//    [SerializeField] public float dashSpeed;

//    [SerializeField] private float speedIncreaseMultiplier;
//    [SerializeField] private float slopeIncreaseMultiplier;

//    public float maxYSpeed;
//    [SerializeField] public float groundDrag;

//    private float moveSpeed;
//    private float desiredMoveSpeed;
//    private float lastDesiredMoveSpeed;
//    private float horizontalInput;
//    private float verticalInput;
//    private Vector3 moveDirection;

//    [Header("Jumping")]
//    [SerializeField] public float jumpForce;
//    [SerializeField] private float jumpCooldown;
//    [Range(0, 2)][SerializeField] private float airMultiplier;

//    [Header("Crouching")]
//    [SerializeField] private float crouchSpeed;
//    [SerializeField] private float crouchYScale;
//    [SerializeField] private float momentumPenalty;
//    private float yScaleOriginal;

//    [Header("Sliding")]
//    [SerializeField] private float maxSlideTime;
//    [SerializeField] private float slideForce;
//    [Range(0, 1)][SerializeField] private float slideSensitivity;
//    private float slideTimer;

//    [Header("Slope Handling")]
//    [SerializeField] private float maxSlopeAngle;
//    private RaycastHit slopeHit;
//    private bool exitingSlope;

//    [Header("Ground Check")]
//    [SerializeField] private float playerHeight;
//    [SerializeField] private LayerMask whatIsGround;

//    [Header("Components")]
//    [SerializeField] private Transform orientation;
//    private Rigidbody rb;

//    [Header("Debug (READ ONLY)")]
//    [SerializeField] private float currentSpeed;
//    [SerializeField] private MovementState state;
//    [SerializeField] private bool jumpAvailable;
//    [SerializeField] public bool grounded;
//    [SerializeField] private bool sliding;
//    [SerializeField] private bool crouched;
//    [SerializeField] public bool dashing;

//    public enum MovementState {
//        walking,
//        dashing,
//        crouching,
//        sliding,
//        air
//    }

//    private void Start() {
//        rb = GetComponent<Rigidbody>();
//        rb.freezeRotation = true;
//        jumpAvailable = true;
//        yScaleOriginal = transform.localScale.y;
//    }

//    private void Update() {
//        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
//        MyInput();
//        SpeedControl();
//        StateHandler();
//        rb.drag = grounded && state != MovementState.dashing ? groundDrag : 0;
//    }

//    private void FixedUpdate() {
//        MovePlayer();
//        if (sliding) SlidingMovement();
//    }

//    private void StateHandler() {
//        if (dashing) {
//            state = MovementState.dashing;
//            moveSpeed = dashSpeed;
//        } else if (sliding) {
//            state = MovementState.sliding;
//            if (OnSlope() && rb.velocity.y < 0.1f)
//                desiredMoveSpeed = slideSpeed;
//            else
//                desiredMoveSpeed = sprintSpeed;
//        } else if (Input.GetKey(crouchKey)) {
//            state = MovementState.crouching;
//            desiredMoveSpeed = crouchSpeed;
//        } else if (grounded) {
//            state = MovementState.walking;
//            desiredMoveSpeed = walkSpeed;
//        } else {
//            state = MovementState.air;
//        }

//        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0) {
//            StopAllCoroutines();
//            StartCoroutine(LerpMoveSpeed());
//        } else {
//            moveSpeed = desiredMoveSpeed;
//        }

//        lastDesiredMoveSpeed = desiredMoveSpeed;
//    }

//    private IEnumerator LerpMoveSpeed() {
//        float time = 0;
//        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
//        float startValue = moveSpeed;

//        while (time < difference) {
//            if (state == MovementState.crouching)
//                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, momentumPenalty * (time / difference));
//            else
//                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

//            if (OnSlope()) {
//                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
//                float slopeAngleIncrease = 1 + (slopeAngle / 90f);
//                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
//            } else {
//                time += Time.deltaTime * speedIncreaseMultiplier;
//            }

//            yield return null;
//        }

//        moveSpeed = desiredMoveSpeed;
//    }

//    private void MyInput() {
//        if (!sliding || slideTimer > maxSlideTime * 0.95f) {
//            horizontalInput = Input.GetAxisRaw("Horizontal");
//            verticalInput = Input.GetAxisRaw("Vertical");
//        }

//        if (Input.GetKey(jumpKey) && jumpAvailable && grounded) {
//            jumpAvailable = false;
//            Jump();
//            Invoke(nameof(ResetJump), jumpCooldown);
//            if (sliding)
//                StopSlide();
//        }

//        if (Input.GetKeyDown(crouchKey)) {
//            if (!sliding && grounded) {
//                StartSlide();
//            } else if (sliding) {
//                StopSlide();
//            }
//        }

//        if (Input.GetKeyUp(crouchKey) && state != MovementState.sliding) {
//            transform.localScale = new Vector3(transform.localScale.x, yScaleOriginal, transform.localScale.z);
//            crouched = false;
//        }

//    }


//    private void MovePlayer() {
//        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

//        if (OnSlope() && !exitingSlope) {
//            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

//            if (rb.velocity.y > 0)
//                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
//        } else if (grounded) {
//            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
//        } else if (!grounded) {
//            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
//        }

//        rb.useGravity = !OnSlope();
//        currentSpeed = moveDirection.magnitude * moveSpeed;
//    }

//    private void SpeedControl() {
//        if (OnSlope() && !exitingSlope) {
//            if (rb.velocity.magnitude > moveSpeed)
//                rb.velocity = rb.velocity.normalized * moveSpeed;
//        } else {
//            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

//            if (flatVelocity.magnitude > moveSpeed) {
//                Vector3 limitedVel = flatVelocity.normalized * moveSpeed;
//                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
//            }
//        }

//        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
//            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.y);
//    }

//    private void Jump() {
//        exitingSlope = true;
//        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
//        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
//    }

//    private void ResetJump() {
//        jumpAvailable = true;
//        exitingSlope = false;
//    }

//    private bool OnSlope() {
//        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
//            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
//            return angle < maxSlopeAngle && angle != 0;
//        }
//        return false;
//    }

//    private Vector3 GetSlopeMoveDirection(Vector3 direction) {
//        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
//    }

//    private void StartSlide() {
//        sliding = true;
//        if (!crouched) {
//            crouched = true;
//            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
//            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
//        }
//    }

//    private void SlidingMovement() {
//        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
//        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
//        slideTimer -= Time.deltaTime;

//        if (slideTimer <= 0)
//            StopSlide();
//    }

//    private void StopSlide() {
//        sliding = false;
//        if (crouched && !Input.GetKey(crouchKey)) {
//            transform.localScale = new Vector3(transform.localScale.x, yScaleOriginal, transform.localScale.z);
//            crouched = false;
//        }
//    }

//}
