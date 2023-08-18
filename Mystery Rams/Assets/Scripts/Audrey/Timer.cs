using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timerStart;
    [SerializeField] float timeTaken;
    [SerializeField] int minutes;
    [SerializeField] int seconds;
    [SerializeField] string time;

    // Start is called before the first frame update
    void Start()
    {
        timerStart = Time.time; 
    }

    // Update is called once per frame
    void Update()
    {
        timeTaken = Time.time - timerStart;

        minutes = Mathf.FloorToInt(timeTaken / 60F);
        seconds = Mathf.FloorToInt(timeTaken - 60F * minutes);
        time = string.Format("{ 0:0}:{ 1:00}", minutes, seconds);
    }
}
 