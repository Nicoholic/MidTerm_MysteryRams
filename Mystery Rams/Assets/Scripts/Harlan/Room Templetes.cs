using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        boss = group.bossRoom;

    }
    public float waitTime;
   // private bool spawnedBoss;
    public GameObject boss;

    
   //public IEnumerator spawnBoss()
   // {

   //     yield return new WaitForSeconds(1.0f);

   //     for (int i = 0; i < rooms.Count; i++)
   //     {
   //         rooms.RemoveAt(rooms.Count - 1);
   //         Instantiate(boss, transform.position, Quaternion.identity);

   //     }


   // }


}
