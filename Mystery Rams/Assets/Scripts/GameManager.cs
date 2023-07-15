using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [Header("KeyBindings")]
    [SerializeField] KeyCode back;

    [Header("Components")]
    public GameObject playerSpawnPoint;
    public GameObject player;
    public GameObject playerCamera;

    [Header("Menu UI")]
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] TextMeshProUGUI enemiesRemainingText;
    [SerializeField] public TextMeshProUGUI heathTxt;
    [SerializeField] public TextMeshProUGUI currentAmmoTxt;
    [SerializeField] public TextMeshProUGUI maxAmmoTxt;
    [SerializeField] public Image PHealthBar;
    [SerializeField] public Image AmmoBar;
    [SerializeField] GameObject playerDamageIndicator;

    int enemiesRemaining;
    public bool isPaused;
    float originalTimeScale;

    void Awake() {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            Debug.LogError("GameManager - No player object with tag 'Player' found.");

        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        if (playerSpawnPoint == null)
            Debug.LogError("GameManager - No playerSpawnPoint object with tag 'PlayerSpawnPoint' found.");

        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera");
        if (playerCamera == null)
            Debug.LogError("GameManager - No PlayerCamera object with tag 'PlayerCamera' found.");

        originalTimeScale = Time.timeScale;
    }

    void Update() {
        if (Input.GetKeyDown(back) && activeMenu == null) {
            PauseGame();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
    }

    

    public void PauseGame() {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    }

    public void UnpauseGame() {
        Time.timeScale = originalTimeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void UpdateGameGoal(int amount) {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");
        if (enemiesRemaining <= 0) {
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            PauseGame();
            UnlockLevel();
        }
    }

    public void GameLoss() {
        PauseGame();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void ReloadUI()
    {                      //replace V if using this
        AmmoBar.fillAmount -= 1.0f / 0 * Time.deltaTime;
    }

    public IEnumerator PlayerHurtFlash() {
        playerDamageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageIndicator.SetActive(false);
    }

    public void SpawnPlayer() {
        player.GetComponent<Rigidbody>().position = playerSpawnPoint.transform.position;
        if (isPaused)
        {
            UnpauseGame();
        }
        GameManager.instance.player.GetComponent<PlayerMovement>().HP = GameManager.instance.player.GetComponent<PlayerMovement>().maxHP;
        player.GetComponent<PlayerMovement>().UpdateUI();
    }

    public void UnlockLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex>=PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
