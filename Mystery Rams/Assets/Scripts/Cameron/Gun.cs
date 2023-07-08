using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject {

    [Header("Gun Stats")]
    public int damage;

    public float timeBetweenShooting;
    public float spread;
    public float range;
    public float reloadTime;
    public float timeBetweenShots;

    public int magazineSize;
    public int bulletsPerTap;
 
    public bool allowButtonHold;

    [Header("Graphics")]
    public GameObject muzzleFlash;
    public GameObject bulletHoleGraphic;




}
