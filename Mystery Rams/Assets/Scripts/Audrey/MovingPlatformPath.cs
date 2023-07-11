using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPath : MonoBehaviour
{
   public Transform GetPoint(int pointIndex) 
    {
    
    return transform.GetChild(pointIndex);
    }

    public int GetNextPointIndex(int currentPointIndex) 
    {
    int nextPointIndex = currentPointIndex + 1;
    if (nextPointIndex == transform.childCount)
        {
        
        nextPointIndex = 0;
        
        }
    return nextPointIndex;
    
    }
}
