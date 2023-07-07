using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] int damage;

    [SerializeField] float timeBetweenShooting;
    [SerializeField] float spread;
    [SerializeField] float range;
    [SerializeField] float reloadTime;
    [SerializeField] float timeBetweenShots;

    [SerializeField] int magazineSize;
    [SerializeField] int bulletsPerTap;

    [SerializeField] bool allowButtonHold;

    private int bulletsLeft;
    private int bulletsShot;

    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    private bool shooting;
    private bool readyToShoot;
    private bool reloading;

    public Camera playerCamera;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    private void MyInput() {
        if (allowButtonHold)
            shooting = Input.GetKey(shootKey);
        else
            shooting = Input.GetKeyDown(shootKey);

        if (Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            Shoot();
    }

    private void Shoot() {

        readyToShoot = false;

        

        bulletsLeft--;
        Invoke(nameof(ResetShot), timeBetweenShooting);
    }

    private void ResetShot() {

    }

    private void Reload() {

    }
}
