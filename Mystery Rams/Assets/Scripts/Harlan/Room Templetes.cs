using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTempletes : MonoBehaviour {
    [SerializeField] RoomGrouping group;

    public GameObject[] northRooms;
    public GameObject[] eastRooms;
    public GameObject[] southRooms;
    public GameObject[] westRooms;

    public GameObject closedRooms;

    public List<GameObject> rooms;

    private void Start() {
        northRooms = group.northRooms;
        eastRooms = group.eastRooms;
        southRooms = group.southRooms;
        westRooms = group.westRooms;
        closedRooms = group.closedRooms;

    }
   
}
