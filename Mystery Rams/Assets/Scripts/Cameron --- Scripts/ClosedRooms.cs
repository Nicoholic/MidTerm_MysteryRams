using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClosedRooms : MonoBehaviour
{
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {

        if (collision.gameObject.layer == 10)
        {

            Destroy(gameObject);
           
        }
        
        
    }
}