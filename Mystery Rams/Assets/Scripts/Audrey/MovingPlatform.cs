using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] MovingPlatformPath path;

    [SerializeField] float platformSpeed;

    [SerializeField] int targetPointIndex;

    [SerializeField] Transform previousPoint;
    [SerializeField] Transform targetPoint;

    [SerializeField] float timeToPoint;
    [SerializeField] float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        TargetNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        float timePercent = timeElapsed / timeToPoint;
        transform.position = Vector3.Lerp(previousPoint.position, targetPoint.position, timePercent);
        if (timeElapsed >= 1) 
        {

            TargetNextPoint();
        
        }
    }

    private void TargetNextPoint()
    {
        previousPoint = path.GetPoint(targetPointIndex);
        targetPointIndex = path.GetNextPointIndex(targetPointIndex);
        targetPoint = path.GetPoint(targetPointIndex);

        timeElapsed = 0;

        float distanceToPoint = Vector3.Distance(previousPoint.position, targetPoint.position);
        timeToPoint = distanceToPoint / platformSpeed;
    }
}
