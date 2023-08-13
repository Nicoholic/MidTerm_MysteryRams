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

    bool initializing;
    int lastListSize;

    private void Start() {
        northRooms = group.northRooms;
        eastRooms = group.eastRooms;
        southRooms = group.southRooms;
        westRooms = group.westRooms;
        closedRooms = group.closedRooms;
        boss = group.bossRoom;
        lastListSize = 0;
        StartRoomCheck();
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

    private void StartRoomCheck() {
        if (initializing)
            return;
        Debug.Log("start");
        StartCoroutine(RoomCheckLoop());
        initializing = true;
    }

    private IEnumerator RoomCheckLoop() {
        lastListSize = rooms.Count;
        Debug.Log("update");
        yield return new WaitForSeconds(0.5f);
        RoomCheck();
    }

    private void RoomCheck() {
        Debug.Log("check");
        Debug.Log("last: " + lastListSize);
        Debug.Log("cur: " + rooms.Count);
        if (lastListSize == rooms.Count) {
            if (rooms[rooms.Count - 1].transform.GetChild(2).GetChild(0).GetChild(4).TryGetComponent<RoomSpawner>(out var roomToReplace)) {
                roomToReplace.DestroyRoom(boss);
                return;
            } else {
                Debug.Log("wrong thing here");
            }
           
        } else
            StartCoroutine(RoomCheckLoop());
    }
}
