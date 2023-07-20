using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] float SpawnSpeed;
    [SerializeField] int maxobj;

    int spawnCount;
    bool isspawning;
    bool startspawn;
    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.UpdateGameGoal(maxobj);
    }

    // Update is called once per frame
    void Update()
    {
        if (startspawn && !isspawning && spawnCount < maxobj)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startspawn = true;

        }
    }

    public IEnumerator spawn()
    {
        isspawning = true;
        spawnCount++;
        Instantiate(objectToSpawn, spawnPos[Random.Range(0, spawnPos.Length)].position, transform.rotation);
        yield return new WaitForSeconds(SpawnSpeed);
        isspawning = false;
    }
}
