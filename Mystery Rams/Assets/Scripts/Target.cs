using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamage {
    public void TakeDamage(int damage) {
        Debug.Log("ouch!");
        Destroy(gameObject);
    }
}
