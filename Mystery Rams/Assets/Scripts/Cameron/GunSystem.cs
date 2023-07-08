using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GunSystem : MonoBehaviour {

    [SerializeField] Gun gun;

    private int damage;
    private float timeBetweenShooting;
    private float spread;
    private float range;
    private float reloadTime;
    private float timeBetweenShots;
    private int magazineSize;
    private int bulletsPerTap;
    private bool allowButtonHold;

    [Header("KeyBindings")]
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    
    private GameObject muzzleFlash;
    private GameObject bulletHoleGraphic;

    [Header("Components")]
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform attackPoint;
    [SerializeField] RaycastHit rayHit;
    [SerializeField] LayerMask whatIsEnemy;

    [Header("Debug")]
    [SerializeField] bool doDebug;

    [SerializeField] int bulletsLeft;
    [SerializeField] int bulletsShot;

    [SerializeField] bool shooting;
    [SerializeField] bool readyToShoot;
    [SerializeField] bool reloading;

    private void Start() {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        if (doDebug)
            Debug.Log("GunSystem - Debug enabled");

        UpdateGunStats();
    }

    private void Update() {
        MyInput();
    }

    private void MyInput() {
        if (allowButtonHold)
            shooting = Input.GetMouseButton(0);
        else
            shooting = Input.GetMouseButtonDown(0);

        if (Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot() {

        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        Vector3 direction = playerCamera.transform.forward + new Vector3(x, y, z);

        if (Physics.Raycast(playerCamera.transform.position, direction, out rayHit, range)) {

            if (doDebug)
                Debug.Log(rayHit.collider.name);

            if (rayHit.collider.TryGetComponent<IDamage>(out var damageable))
                damageable.TakeDamage(damage);
        }

        bulletsLeft--;
        bulletsShot--;

        if (bulletHoleGraphic != null)
            Instantiate(bulletHoleGraphic, rayHit.point + new Vector3(0f, 0f, 0f), Quaternion.LookRotation(-rayHit.normal));

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        if (!IsInvoking(nameof(ResetShot)) && !readyToShoot)
            Invoke(nameof(ResetShot), timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    private void ResetShot() {
        readyToShoot = true;
    }

    private void Reload() {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished() {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public void UpdateGunStats() {
        damage = gun.damage;
        timeBetweenShooting = gun.timeBetweenShooting;
        spread = gun.spread;
        range = gun.range;
        reloadTime = gun.reloadTime;
        timeBetweenShots = gun.timeBetweenShots;
        magazineSize = gun.magazineSize;
        bulletsPerTap = gun.bulletsPerTap;
        allowButtonHold = gun.allowButtonHold;
        muzzleFlash = gun.muzzleFlash;
        bulletHoleGraphic = gun.bulletHoleGraphic;
    }
}
