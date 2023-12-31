using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [Header("KeyBindings")]
    [SerializeField] KeyCode back;

    [Header("Components")]
    public GameObject playerSpawnPoint;
    public GameObject player;
    public GameObject playerCamera;

    [Header("Menu UI")]
    [SerializeField] public GameObject activeMenu;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] GameObject statsMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] TextMeshProUGUI enemiesRemainingText;
    [SerializeField] public TextMeshProUGUI heathTxt;
    [SerializeField] public TextMeshProUGUI currentAmmoTxt;
    [SerializeField] public TextMeshProUGUI maxAmmoTxt;
    [SerializeField] public TextMeshProUGUI StaminaTxt;
    [SerializeField] public Image PHealthBar;
    [SerializeField] public Image PStaminaBar;
    [SerializeField] GameObject playerDamageIndicator;
    [SerializeField] public GameObject hitmarker;
    [SerializeField] public GameObject gunPickUpGUI;

    int enemiesRemaining;
    public bool isPaused;
    public bool isStats;
    public float originalTimeScale;

    public float Stamina;
    public float mStamina;
    public float Rcharge;
    public Coroutine recharge;

    public PlayerMovement playerMovement;


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

        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();

    }


    void Update() {
        if (Input.GetKeyDown(back) && activeMenu == null) {
            PauseGame();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
        if (Input.GetKeyDown(KeyCode.Tab) && activeMenu == null)
        {
            activeMenu = statsMenu;
            activeMenu.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            activeMenu.SetActive(false);
            activeMenu = null;
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

    public void GameLoss() {
        PauseGame();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void GameWin() {
        PauseGame();    
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }

    public void LevelUnlocked()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = currentLevel + 1;

        int OpenLevel = PlayerPrefs.GetInt("OpenLevel", 1);
        if (nextLevel > OpenLevel)
        {
            PlayerPrefs.SetInt("OpenLevel", nextLevel);
        }
    }

    public IEnumerator PlayerHurtFlash() {
        playerDamageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageIndicator.SetActive(false);
    }

    public IEnumerator StaminaCharge()
    {
        //yield return new WaitForSeconds(1f);
        while(Stamina < mStamina) 
        {
            Stamina += Rcharge / 10f;
            if(Stamina > mStamina)
            {
                Stamina = mStamina;
            }
            PStaminaBar.fillAmount = Stamina / mStamina;
            GameManager.instance.StaminaTxt.text = Stamina.ToString("F0");
            yield return new WaitForSeconds(.1f);
        }
    }
    public IEnumerator Hitmark()
    {
    hitmarker.SetActive(true);
    yield return new WaitForSeconds(0.2f);
    hitmarker.SetActive(false);
    }

}
