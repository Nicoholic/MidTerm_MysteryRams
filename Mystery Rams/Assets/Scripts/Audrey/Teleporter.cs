using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{


    
    void Start()
    {

    }

    
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
       other.transform.position = new Vector3 (Random.Range (-10.0f, 10.0f), other.transform.position.y, Random.Range(-10.0f, 10.0f));
    }
}
