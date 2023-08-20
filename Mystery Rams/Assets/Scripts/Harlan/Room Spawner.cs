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
    [SerializeField] private bool spawned = false;
    public bool bossSpawned = false;
    public float waitTime = 5f;
    bool isGround = false;
    [SerializeField] GameObject[] spawnPointObjects;
    
    



    private void Start()
    {

        Destroy(gameObject, waitTime);
        templetes = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTempletes>();
        spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Invoke("Spawn", 0.2f);
        





    }





    void Spawn()
    {

        
        if (spawned == false )
        {


            // 1 = North opening 
            if (openingDirection == 1)
            {

                rand = Random.Range(0, templetes.northRooms.Length);
                Instantiate(templetes.northRooms[rand], transform.position, Quaternion.identity);
                //AreAllSpawnPointsOverEmptySpace();


            }
            // 2 = East opening 
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, templetes.eastRooms.Length);
                Instantiate(templetes.eastRooms[rand], transform.position, Quaternion.identity);
                //AreAllSpawnPointsOverEmptySpace();
            }
            // 3 = South opening
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templetes.southRooms.Length);
                Instantiate(templetes.southRooms[rand], transform.position, Quaternion.identity);
               // AreAllSpawnPointsOverEmptySpace();
            }
            // 4 = West opening 
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templetes.westRooms.Length);
                Instantiate(templetes.westRooms[rand], transform.position, Quaternion.identity);
                //AreAllSpawnPointsOverEmptySpace();
            }

            spawned = true;
            AreAllSpawnPointsOverEmptySpace();
        }



    }


    private void OnTriggerEnter(Collider other)
    {
        bool canSpawnRoom = AreAllSpawnPointsOverEmptySpace();
        
        if (openingDirection == 0)
            spawned = true;



        if (other.CompareTag("SpawnPoint"))
        {


            if (other.TryGetComponent<RoomSpawner>(out var closedRoomSpawn) && closedRoomSpawn.spawned == false && spawned == false && canSpawnRoom) 
            {
               
                
                  Instantiate(templetes.closedRooms, transform.position, Quaternion.identity);
                  Destroy(gameObject);
                
                
                

            }
            spawned = true;
        }


    }

    public void DestroyRoom(GameObject roomToSpawn)
    {
        Instantiate(roomToSpawn, transform.position, Quaternion.identity);
        Destroy(templetes.rooms[^1]);
    }

    private void DrawRay(Vector3 origin, Vector3 end, Vector3 direction, Color color)
    {
        Debug.DrawLine(origin, end, color, 10);
        Debug.DrawLine(origin, end + (Vector3.up + Vector3.right) * 0.2f, color, 10);
        Debug.DrawLine(origin, end + (Vector3.up + Vector3.left) * 0.2f, color, 10);
    }

    
    private bool AreAllSpawnPointsOverEmptySpace()
    {
        foreach(GameObject spawnPointObject in spawnPointObjects)
    {
            Transform spawnPointTransform = spawnPointObject.transform;

            RaycastHit hit;

            if (Physics.Raycast(spawnPointTransform.position, Vector3.down, out hit, 10f) && Physics.Raycast(spawnPointTransform.position, Vector3.up, out hit, 10f))
            {
                if (hit.collider.gameObject.layer == 10)
                {
                    Debug.Log("Ray Hit Ground");
                    DrawRay(spawnPointTransform.position, hit.point, Vector3.down, Color.green);
                    DrawRay(spawnPointTransform.position, hit.point, Vector3.up, Color.green);
                    return false;
                }
            }
            else
            {
                Debug.Log("No Ground");
                DrawRay(spawnPointTransform.position, spawnPointTransform.position + Vector3.down * 10f, Vector3.down, Color.red);
                DrawRay(spawnPointTransform.position, spawnPointTransform.position + Vector3.up * 10f, Vector3.down, Color.red);
            }
        }

        Debug.Log("All Spawn Points Over Empty Space");
        return true;
    }
}






