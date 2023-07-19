using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {
    public void Resume() {
        GameManager.instance.UnpauseGame();
    }

    public void Respawn() {
        GameManager.instance.player.GetComponent<PlayerMovement>().SpawnPlayer();
    }

    public void Restart() {
        GameManager.instance.UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        GameManager.instance.LevelUnlocked();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.instance.player.GetComponent<PlayerMovement>().SpawnPlayer();
        GameManager.instance.player.GetComponent<PlayerMovement>().HP = GameManager.instance.player.GetComponent<PlayerMovement>().maxHP;
        GameManager.instance.player.GetComponent<PlayerMovement>().UpdateUI();
    }
}
