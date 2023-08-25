using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {

    [SerializeField] private PlayerSettingsManager settingsManager;
    public void Resume() {
        GameManager.instance.UnpauseGame();
    }

    public void Restart() {
        GameManager.instance.UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        Time.timeScale = 1;

        if (GameManager.instance.player.TryGetComponent<PlayerMovement>(out PlayerMovement player)) {
            player.SpawnPlayer();
            player.HP = player.maxHP;
            player.UpdateUI();
        }
    }

    public void TutorialLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
        if (GameManager.instance.player.TryGetComponent<PlayerMovement>(out PlayerMovement player)) {
            player.SpawnPlayer();
            player.HP = player.maxHP;
            player.UpdateUI();
        }
    }
}
