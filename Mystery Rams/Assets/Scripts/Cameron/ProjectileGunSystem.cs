using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGunSystem : MonoBehaviour {

    [Header("Bullet")]
    public GameObject bullet;

    [Header("Bullet")]
    public float shootForce;
    public float upwardForce;

    [Header("Gun Stats")]
    public float timeBetweenShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;
    public int magazineSize;
    public float bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft;
    int bulletsShot;

    [Header("Recoil")]
    public Rigidbody playerRb;
    public float recoilForce;

    [Header("Bool")]
    bool shooting;
    bool readyToShoot;
    bool reloading;

    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;

    [Header("Graphics")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    [Header("Debug")]
    public bool allowInvoke = true;

    void Start() {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    void Update() {
        MyInput();
    }

    private void MyInput() {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) 
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            Reload();
 
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0) {

            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot() {
        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); 

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); 

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke) {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;

            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    private void ResetShot() {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload() {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime); 
    }
    private void ReloadFinished() {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
