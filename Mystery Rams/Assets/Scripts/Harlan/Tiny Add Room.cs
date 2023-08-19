using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyAddRoom : MonoBehaviour
{
    private RoomTempletes tinyTemplates;
    // Start is called before the first frame update
    void Start()
    {
        tinyTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTempletes>();
        tinyTemplates.rooms.Add(this.gameObject);


    }

    
}
