using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject teleportLocation;
    [SerializeField] GameObject teleportStart;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Teleporter"))
        {
            player.transform.position = teleportLocation.transform.position;

        }

        if (other.gameObject.CompareTag("Second Teleporter")) 
        {
        
        player.transform.position =  teleportStart.transform.position;
        
        }
    }
}
