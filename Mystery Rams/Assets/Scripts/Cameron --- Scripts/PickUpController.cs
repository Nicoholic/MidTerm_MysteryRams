using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour {

    public ProjectileGun gun;
    public Rigidbody rb;
    public BoxCollider bc;
    public Transform player;
    public Transform gunContainer;
    public Transform playerCamera;

    public float pickUpRange;
    public float dropForwardForce;
    public float dropUpwardForce;

    public bool equipped;
    public static bool slotFull = false;

    KeyCode pickUp = KeyCode.F;
    KeyCode drop = KeyCode.Q;


    private void Start() {
        player = GameManager.instance.player.transform;
        playerCamera = GameManager.instance.playerCamera.transform;
        gunContainer = GameManager.instance.playerCamera.transform.GetChild(0);

        if(!equipped) {
            gun.enabled = false;
            rb.isKinematic = false;
            bc.isTrigger = false;
        } else if (equipped) {
            gun.enabled = true;
            rb.isKinematic = true ;
            bc.isTrigger = true ;
            slotFull = true;
        }

    }

    private void Update() {
        Vector3 distanceToPlayer = player.position - transform.position;

        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(pickUp) && !slotFull)
            PickUp();

        if (equipped && Input.GetKeyDown(drop))
            Drop();
    }

    private void PickUp() {
        equipped = true;
        slotFull = true;

        transform.SetParent(gunContainer);
        transform.SetLocalPositionAndRotation(gun.offset, Quaternion.Euler(Vector3.zero));
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        bc.isTrigger = true;

        gun.enabled = true;
    }

    private void Drop() {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        bc.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(playerCamera.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(playerCamera.forward * dropUpwardForce, ForceMode.Impulse);

        float random = UnityEngine.Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        gun.enabled = false;

    }
}
