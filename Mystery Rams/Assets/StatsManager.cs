using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class StatsManager : MonoBehaviour
{
    private static StatsManager _instance;

    public static StatsManager Instance { get { return _instance; } }


    int minutes;
    int seconds;
    public string formattedTime;

    float shotsFired;
    float shotsHit;
    public string formattedAccurcy;

    int enemiesKilled;
    public string formattedEnemiesKilled;

    int playerDamage;
    public string formattedPlayerDamage;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI playerDamageText;

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }


    void Start()
    {
        seconds = 0;
        minutes = 0;
        shotsFired = 0;
        shotsHit = 0;
        enemiesKilled = 0;
        playerDamage = 0;

        formattedTime = "00:00";
        formattedAccurcy = "0.00%";
        formattedEnemiesKilled = "0";
        formattedPlayerDamage = "0";

        StartCoroutine(CountSecond());
    }

    private void ChangeUI()
    {
        timeText.text = formattedTime;

        accuracyText.text = formattedAccurcy;

        enemiesKilledText.text = formattedEnemiesKilled;

        playerDamageText.text = formattedPlayerDamage;
    }


    private IEnumerator CountSecond()
    {
        yield return new WaitForSeconds(1);
        seconds++;
        minutes = Mathf.FloorToInt(seconds / 60);
        FormatTime();

        StartCoroutine(CountSecond());
    }

    private void FormatTime()
    {
        string formattedSeconds;
        string formattedMinutes;

        if (seconds % 60 < 10)
        {
            formattedSeconds = "0" + seconds % 60;
        }
        else
        {
            formattedSeconds = "" + seconds % 60;
        }

        if (minutes < 10)
        {
            formattedMinutes = "0" + minutes;
        }
        else
        {
            formattedMinutes = "" + minutes;
        }

        formattedTime = formattedMinutes + ":" + formattedSeconds;
        ChangeUI();
    }

    public void ShotFired()
    {
        shotsFired++;
        FormatAccuracy();
    }

    public void ShotHit()
    {
        shotsHit++;
        FormatAccuracy();
    }

    private void FormatAccuracy()
    {
        formattedAccurcy = "" + (Mathf.Round((shotsHit / shotsFired)*10000) / 100) + "%";
        ChangeUI();
    }

    public void KilledEnemy()
    {
        enemiesKilled++;
        formattedEnemiesKilled = "" + enemiesKilled;
        ChangeUI();
    }

    public void PlayerTookDamage(int damageAmount)
    {
        playerDamage += damageAmount;
        formattedPlayerDamage = "" + playerDamage;
        ChangeUI();
    }
}
