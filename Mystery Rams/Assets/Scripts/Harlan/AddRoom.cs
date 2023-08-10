using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    // Start is called before the first frame update
    private RoomTempletes templates;
    //[SerializeField] int RoomCap;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTempletes>();
        templates.rooms.Add(this.gameObject);
        
        
    }
}
