using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {
    public void Resume() {
        GameManager.instance.UnpauseGame();
    }

    public void Respawn() {
        GameManager.instance.UnpauseGame();
        GameManager.instance.SpawnPlayer();
    }

    public void Restart() {
        GameManager.instance.UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit() {
        Application.Quit();
    }
}
