using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Timer : MonoBehaviour
{
    int minutes;
    int seconds;
    public string formattedTime;

    void Start()
    {
        seconds = 0;
        minutes = 0;
        StartCoroutine(CountSecond());
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
    }
}
