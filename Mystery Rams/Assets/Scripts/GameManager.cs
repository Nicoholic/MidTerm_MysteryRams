using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Menu UI")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public TextMeshProUGUI enemiesRemainingText;
    public GameObject playerDamageIndicator;

    int enemiesRemaining;
    bool isPaused;
    float timeScaleOrig;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null) 
        {
        PauseGame();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        
        }
        
    }

    public void PauseGame() 
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    
    }

    public void GameUnpaused() 
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
    
    }
    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");
        if(enemiesRemaining <= 0)
        {
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            PauseGame();
        }
    }
    public void GameLoss()
    {
        PauseGame();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    public IEnumerator playerDamageIndication() 
    {
    playerDamageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageIndicator.SetActive(false);
    
    
    }
}
