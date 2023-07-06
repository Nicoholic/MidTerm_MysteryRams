using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {
    public void Resume() {
        GameManager.instance.UnpauseGame();
    }

    public void Respawn() {
        GameManager.instance.SpawnPlayer();
        GameManager.instance.player.GetComponent<PlayerShoot>().HP = GameManager.instance.player.GetComponent<PlayerShoot>().maxHP;
        GameManager.instance.player.GetComponent<PlayerShoot>().PlayerUiUpdate();
    }

    public void Restart() {
        GameManager.instance.UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit() {
        Application.Quit();
    }
}
