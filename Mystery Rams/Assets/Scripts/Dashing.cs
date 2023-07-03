//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;

//public class Dashing : MonoBehaviour {
//    [Header("Components")]
//    public Transform orientation;
//    public Transform playerCam;
//    private Rigidbody rb;
//    private PlayerMovement pm;

//    [Header("Dashing")]
//    public float dashForce;
//    public float dashUpwardForce;
//    public float maxDashYSpeed;
//    public float dashDuration;

//    [Header("Camera Effects")]
//    public PlayerCamera playerCamera;
//    public float dashingFov;

//    [Header("Settings")]
//    public bool useCameraForward = true;
//    public bool disableGravity = false;
//    public bool resetVel = true;

//    [Header("Cooldown")]
//    public float dashCooldown;
//    private float dashCooldownTimer;

//    [Header("Input")]
//    public KeyCode dashKey = KeyCode.LeftShift;

//    private bool isDashing = false;
//    private Vector3 dashDirection;

//    private void Start() {
//        rb = GetComponent<Rigidbody>();
//        pm = GetComponent<PlayerMovement>();
//    }

//    private void Update() {
//        if (Input.GetKeyDown(dashKey))
//            TryDash();

//        if (dashCooldownTimer > 0)
//            dashCooldownTimer -= Time.deltaTime;
//    }

//    private void TryDash() {
//        if (dashCooldownTimer > 0 || pm.dashing)
//            return;

//        Dash();
//    }

//    private void Dash() {
//        dashCooldownTimer = dashCooldown;
//        pm.dashing = true;
//        pm.maxYSpeed = maxDashYSpeed;
//        isDashing = true;

//        playerCamera.DoFov(dashingFov);

//        dashDirection = GetDirection();

//        if (disableGravity)
//            rb.useGravity = false;

//        if (resetVel)
//            rb.velocity = Vector3.zero;

//        Invoke(nameof(ResetDash), dashDuration);
//    }

//    private void FixedUpdate() {
//        if (isDashing) {
//            Vector3 dashForceVector = dashDirection * dashForce;
//            dashForceVector.y += dashUpwardForce;

//            rb.AddForce(dashForceVector, ForceMode.Impulse);
//        }
//    }

//    private void ResetDash() {
//        pm.dashing = false;
//        pm.maxYSpeed = 0;
//        isDashing = false;

//        playerCamera.DoFov(85f);

//        if (disableGravity)
//            rb.useGravity = true;
//    }

//    private Vector3 GetDirection() {
//        float horizontalInput = Input.GetAxisRaw("Horizontal");
//        float verticalInput = Input.GetAxisRaw("Vertical");

//        Vector3 direction = Vector3.zero;

//        if (useCameraForward && !isDashing)
//            direction = (playerCam.forward * verticalInput) + (playerCam.right * horizontalInput);
//        else
//            direction = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

//        if (direction == Vector3.zero)
//            direction = orientation.forward;

//        return direction.normalized;
//    }
//}