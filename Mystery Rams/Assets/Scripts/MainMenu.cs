using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Starts the game by going to next Scene
    public void GamePlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    //Quits the game
    public void GameQuit()
    {
        Application.Quit();
    }
}
