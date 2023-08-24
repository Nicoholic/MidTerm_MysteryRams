using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClosedRooms : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.layer == 10)
        {

            Destroy(gameObject);
           
        }
        
        
    }
}