using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunInfo : ScriptableObject {

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

}
