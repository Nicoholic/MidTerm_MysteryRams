using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGunSystem : MonoBehaviour {

    [Header("Gun")]
    [SerializeField] Gun gun;

    public TextMeshProUGUI ammunitionDisplay;

    [Header("Debug")]
    [SerializeField] bool allowInvoke = true;
    [SerializeField] int bulletsLeft;
    [SerializeField] int bulletsShot;
    [SerializeField] bool shooting;
    [SerializeField] bool readyToShoot;
    [SerializeField] bool reloading;

    private Rigidbody rb;
    private Camera playerCamera;


    void Start() {
        bulletsLeft = gun.gunInfo.magazineSize;
        readyToShoot = true;
        playerCamera = GameManager.instance.playerCamera.GetComponent<Camera>();
        rb = GameManager.instance.player.GetComponent<Rigidbody>();
        gun.transform.parent = playerCamera.transform;
        gun.transform.localPosition = Vector3.zero;
    }

    void Update() {
        MyInput();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / gun.gunInfo.bulletsPerTap + "/" + gun.gunInfo.magazineSize / gun.gunInfo.bulletsPerTap);
    }

    private void MyInput() {
        if (gun.gunInfo.allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < gun.gunInfo.magazineSize && !reloading)
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

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - gun.attackPoint.position;

        float x = Random.Range(-gun.gunInfo.spread, gun.gunInfo.spread);
        float y = Random.Range(-gun.gunInfo.spread, gun.gunInfo.spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(gun.gunInfo.bullet, gun.attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * gun.gunInfo.shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(playerCamera.transform.up * gun.gunInfo.upwardForce, ForceMode.Impulse);

        if (gun.muzzleFlash != null)
            Instantiate(gun.muzzleFlash, gun.attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke) {
            Invoke(nameof(ResetShot), gun.gunInfo.timeBetweenShooting);
            allowInvoke = false;

            rb.AddForce(-directionWithSpread.normalized * gun.gunInfo.recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot < gun.gunInfo.bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), gun.gunInfo.timeBetweenShots);
    }

    private void ResetShot() {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload() {
        reloading = true;
        Invoke(nameof(ReloadFinished), gun.gunInfo.reloadTime);
    }
    private void ReloadFinished() {
        bulletsLeft = gun.gunInfo.magazineSize;
        reloading = false;
    }
}
