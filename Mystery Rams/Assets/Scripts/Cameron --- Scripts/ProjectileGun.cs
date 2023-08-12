using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGun : MonoBehaviour {

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


    public float recoilForce;

    public Transform attackPoint;
    public GameObject muzzleFlash;

    public Vector3 offset;

    [Header("Gun Image")]
    public Sprite gunImage;

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
        bulletsLeft = magazineSize;
        readyToShoot = true;
        playerCamera = GameManager.instance.playerCamera.GetComponent<Camera>();
        rb = GameManager.instance.player.GetComponent<Rigidbody>();
    }

    void Update() {
        MyInput();
    }

    private void FixedUpdate() {
        GameManager.instance.currentAmmoTxt.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
    }

    private void MyInput() {
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

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

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

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
        currentBullet.GetComponent<Rigidbody>().AddForce(playerCamera.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke) {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;

            rb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
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
