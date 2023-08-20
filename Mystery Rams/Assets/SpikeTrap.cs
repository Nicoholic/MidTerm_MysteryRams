using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SpikeTrap : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float moveDuration;
    [SerializeField] float pauseDuration;

    [Header("Componets")]
    [SerializeField] GameObject startingPoint;
    [SerializeField] GameObject spikes;

    Vector3 endPoint;
    Vector3 startPoint;

    float elapsedTimeUp = 0.0f;
    float elapsedTimeDown = 0.0f;

    bool goingUp;

    void Start()
    {
        endPoint = spikes.transform.position;
        startPoint = startingPoint.transform.position;
        spikes.transform.position = startPoint;
        goingUp = true;
    }

    private void Update()
    {
        if (goingUp)
        {
            SpikesUp();
        }
        else
        {
            SpikesDown();
        }
    }

    private void SpikesUp()
    {
        elapsedTimeUp += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTimeUp / moveDuration);
        spikes.transform.position = Vector3.Lerp(startPoint, endPoint, t);

        if (Vector3.Distance(spikes.transform.position, endPoint) < 0.001f)
        {
            FlipState();
        }
    }

    private void SpikesDown()
    {
        elapsedTimeDown += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTimeDown / moveDuration);
        spikes.transform.position = Vector3.Lerp(endPoint, startPoint, t);

        if (Vector3.Distance(spikes.transform.position, startPoint) < 0.001f)
        {
            FlipState();
        }
    }

    private void FlipState()
    {
        goingUp = !goingUp;
        elapsedTimeUp = 0.0f;
        elapsedTimeDown = 0.0f;
    }
}
