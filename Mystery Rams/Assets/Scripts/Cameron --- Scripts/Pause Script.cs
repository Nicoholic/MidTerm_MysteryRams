using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {

    [SerializeField] AggroEnemy pauseMe;
    [SerializeField] float howLong;
    void Start() {
        pauseMe.enabled = false;
        Invoke(nameof(UnpauseMe), howLong);
    }

    private void UnpauseMe() {
        pauseMe.enabled = true;
    }
}
