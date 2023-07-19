using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] DoorManager door;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb)) 
        {
        if(!door.isOpen) 
            {
                door.Open(other.transform.position);
            }
        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb)) 
        {
            if (door.isOpen) 
            {

                door.Close();
            
            }
        
        }
    }
}
