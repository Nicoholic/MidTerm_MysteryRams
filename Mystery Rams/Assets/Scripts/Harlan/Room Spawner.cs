using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection;
    // 1 = North opening 
    // 2 = East opening 
    // 3 = South opening
    // 4 = West opening 

    private RoomTempletes templetes;
    private int rand;
    private bool spawned = false;
    public bool bossSpawned = false;
    public float waitTime = 2f;
    [SerializeField] int bossRoomDirection;




    private void Start()
    {
        Destroy(gameObject, waitTime);
        bossRoomDirection = Random.Range(5, 8);
        templetes = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTempletes>();
        Invoke("Spawn", 0.2f);

    }



    void Spawn()
    {
        if (spawned == false)
        {
            // 1 = North opening 
            if (openingDirection == 1)
            {
                rand = Random.Range(0, templetes.northRooms.Length);
                Instantiate(templetes.northRooms[rand], transform.position, Quaternion.identity);


            }
            // 2 = East opening 
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, templetes.eastRooms.Length);
                Instantiate(templetes.eastRooms[rand], transform.position, Quaternion.identity);

            }
            // 3 = South opening
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templetes.southRooms.Length);
                Instantiate(templetes.southRooms[rand], transform.position, Quaternion.identity);

            }
            // 4 = West opening 
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templetes.westRooms.Length);
                Instantiate(templetes.westRooms[rand], transform.position, Quaternion.identity);

            }
            if (openingDirection == 5 || openingDirection == 6 || openingDirection == 7 || openingDirection == 8)
            {
                Debug.Log("Bossroom Direction: " + bossRoomDirection);
                Debug.Log("Opening Direction: " + openingDirection);

                if (!bossSpawned && openingDirection == bossRoomDirection)
                {
                    Instantiate(templetes.boss, transform.position, Quaternion.identity);
                    bossSpawned = true;
                }



            }

            spawned = true;

        }


    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.TryGetComponent<RoomSpawner>(out var closedRoomSpawn) && closedRoomSpawn.spawned == false && spawned == false )
            {
                Instantiate(templetes.closedRooms, transform.position, Quaternion.identity);
                Destroy(this.gameObject);

            }
            spawned = true;
        }


    }

    public void DestroyRoom(GameObject roomToSpawn)
    {
        Instantiate(roomToSpawn, transform.position, Quaternion.identity);
        Destroy(templetes.rooms[^1]);
    }




}
