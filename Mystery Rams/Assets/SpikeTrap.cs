using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SpikeTrap : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float moveDurationUp;
    [SerializeField] float moveDurationDown;
    [SerializeField] float pauseDuration;

    [Header("Components")]
    [SerializeField] GameObject startingPoint;
    [SerializeField] GameObject spikes;

    Vector3 endPoint;
    Vector3 startPoint;

    float elapsedTime = 0.0f;
    bool goingUp = true;

    void Start()
    {
        endPoint = spikes.transform.position;
        startPoint = startingPoint.transform.position;
        spikes.transform.position = startPoint;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (goingUp)
        {
            MoveSpikes(startPoint, endPoint, moveDurationUp);
        }
        else
        {
            MoveSpikes(endPoint, startPoint, moveDurationDown);
        }
    }

    private void MoveSpikes(Vector3 Movefrom, Vector3 Moveto, float duration)
    {
        float t = Mathf.Clamp01(elapsedTime / duration);
        spikes.transform.position = Vector3.Lerp(Movefrom, Moveto, t);

        if (t >= 1.0f)
        {
            if (goingUp)
                StartCoroutine(PauseCoroutine());
            else
                StartCoroutine(MoveDownCoroutine());
        }
    }

    IEnumerator PauseCoroutine()
    {
        yield return new WaitForSeconds(pauseDuration);
        goingUp = false;
        elapsedTime = 0.0f;
    }

    IEnumerator MoveDownCoroutine()
    {
        yield return new WaitForSeconds(pauseDuration);
        goingUp = true;
        elapsedTime = 0.0f;
    }
}
