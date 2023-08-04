using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        int OpenLevel = PlayerPrefs.GetInt("OpenLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < OpenLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void LevelJoin(int levelID)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + levelID);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
