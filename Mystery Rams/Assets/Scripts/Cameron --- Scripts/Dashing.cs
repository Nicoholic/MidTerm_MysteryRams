using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour {
    [Header("Components")]
    public Transform orientation;
    public Transform cameraPosition;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("Camera Effects")]
    public PlayerCamera playerCamera;
    public float dashingFov;

    private float originalFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCooldown;
    private float dashCooldownTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    private bool isDashing = false;
    private Vector3 dashDirection;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        playerCamera = GameManager.instance.playerCamera.GetComponent<PlayerCamera>();
        cameraPosition = GameManager.instance.player.transform.GetChild(1);
        originalFov = playerCamera.GetComponent<Camera>().fieldOfView;
    }

    private void Update() {
        if (Input.GetKeyDown(dashKey) && !GameManager.instance.isPaused)
        {
            TryDash();
           
            if(GameManager.instance.Stamina < 0) GameManager.instance.Stamina = 0;
            GameManager.instance.PStaminaBar.fillAmount = GameManager.instance.Stamina / GameManager.instance.mStamina;

            if (GameManager.instance.recharge != null) StopCoroutine(GameManager.instance.recharge);
            GameManager.instance.recharge = StartCoroutine(GameManager.instance.StaminaCharge());
        }
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    private void TryDash() {
        if (dashCooldownTimer > 0 || pm.dashing)
            return;

        Dash();
    }

    private void Dash() {
        pm.DashSound.Play();
        dashCooldownTimer = dashCooldown;
        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;
        isDashing = true;

        GameManager.instance.Stamina -= 100;

        StartCoroutine(playerCamera.LerpFov(dashingFov));

        dashDirection = GetDirection();

        if (disableGravity)
            rb.useGravity = false;

        if (resetVel)
            rb.velocity = Vector3.zero;

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void FixedUpdate() {
        if (isDashing) {
            Vector3 dashForceVector = dashDirection * dashForce;
            dashForceVector.y += dashUpwardForce;

            rb.AddForce(dashForceVector, ForceMode.Impulse);
        }
    }

    private void ResetDash() {
        pm.dashing = false;
        pm.maxYSpeed = 0;
        isDashing = false;
        StartCoroutine(
                playerCamera.LerpFov(originalFov));
        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction;

        if (useCameraForward && !isDashing)
            direction = (cameraPosition.forward * verticalInput) + (cameraPosition.right * horizontalInput);
        else
            direction = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        if (direction == Vector3.zero)
            direction = orientation.forward;

        return direction.normalized;
    }
}