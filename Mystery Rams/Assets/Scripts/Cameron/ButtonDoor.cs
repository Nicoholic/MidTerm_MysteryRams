using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamage {

    [SerializeField] GameObject thingToDestroy;
    public void TakeDamage(int damage) {
        Destroy(thingToDestroy);
    }
}
