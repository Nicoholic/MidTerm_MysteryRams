using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnTinyRoom : MonoBehaviour
{
    [SerializeField] GameObject RoomToSpawn;
    public List<GameObject> tinyRooms;
    private bool reSpawned = true;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("start");
        StartCoroutine(nameof(ReSpawnLoop));
          
       
    }
    
    IEnumerator ReSpawnLoop()
    {
        Debug.Log("Respawn");
        yield return new WaitForSeconds(10.0f);
        RoomRespawn();

    }
    private void RoomRespawn()
    {
        if (tinyRooms.Count == 0)
        {
            Instantiate(RoomToSpawn, transform.position, Quaternion.identity);
            return;
        }
        else
        {
            StartCoroutine(nameof(ReSpawnLoop));
        }
        
    }
    

    // Update is called once per frame

}
