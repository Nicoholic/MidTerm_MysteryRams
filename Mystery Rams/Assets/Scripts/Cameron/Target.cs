using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamage {
    public void TakeDamage(int damage) {
        Destroy(gameObject);
    }
}
