using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dashing : MonoBehaviour {

    [Header("Components")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("CameraEffects")]
    public PlayerCamera playerCamera;
    public float dashingFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCooldown;
    private float dashCooldownTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.LeftShift;

    void Start() {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update() {

        if (Input.GetKeyDown(dashKey))
            Dash();

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

    }



    private void Dash() {

        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        playerCamera.DoFov(dashingFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam;
        else
            forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = (direction * dashForce) +
                                (orientation.up * dashUpwardForce);

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce() {

        if (resetVel)
            rb.velocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash() {

        pm.dashing = false;
        pm.maxYSpeed = 0;

        playerCamera.DoFov(85f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT) {

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
