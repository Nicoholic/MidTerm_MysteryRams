using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection;
    // 1 = North opening 
    // 2 = East opening 
    // 3 = South opening
    // 4 = West opening 

    private RoomTempletes templetes;
    private int rand;
    [SerializeField] private bool spawned = false;
    public bool bossSpawned = false;
    public float waitTime = 5.0f;
    
    





    private void Start()
    {

        Destroy(gameObject, waitTime);
        templetes = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTempletes>();
        //spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Invoke(nameof(Spawn), 0.1f);
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

            spawned = true;
           
        }



    }


    private void OnTriggerEnter(Collider other)
    {
        // bool canSpawnRoom = AreAllSpawnPointsOverEmptySpace(spawnPointObject);

        if (openingDirection == 0)
            spawned = true;



        if (other.CompareTag("SpawnPoint"))
        {


            if (other.TryGetComponent<RoomSpawner>(out var closedRoomSpawn) && closedRoomSpawn.spawned == false && spawned == false)
            {

                if (templetes.closedRooms.gameObject != null)
                {
                    Instantiate(templetes.closedRooms, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }




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






