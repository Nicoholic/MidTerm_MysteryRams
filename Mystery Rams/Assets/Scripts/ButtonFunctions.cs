using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume() 
    {

    GameManager.instance.PauseGame();

    }

    public void Restart()
    {
        GameManager.instance.GameUnpaused();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void Respawn()
    //{
    //GameManager.instance.GameUnpause();
    //
    //}
    
    public void Quit()
    {
        Application.Quit();
    }
}
