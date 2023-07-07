using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    [SerializeField] float timer;
    void Start() {
        Invoke(nameof(Destruct), timer);
    }

    void Destruct() {
        Destroy(gameObject);
    }
}
