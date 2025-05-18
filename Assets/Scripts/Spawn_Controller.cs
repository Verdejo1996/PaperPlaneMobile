using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Controller : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject airCurrent;
    public float spawnRate = 2f;
    public float spawnRangeX = 3f;
    public float spawnHeight = 5f;
    public float spawnInterval = 2f;

    //private bool isActive = false;
    //private float timer;

    void Start()
    {
        InvokeRepeating(nameof(SpawnElement), 1f, spawnInterval);
    }
/*    void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Vector3 spawnPos = new(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(0f, spawnHeight), 25f);
            GameObject toSpawn = Random.value > 0.7f ? airCurrent : obstacles[Random.Range(0, obstacles.Length)];
            Instantiate(toSpawn, spawnPos, Quaternion.identity);
            timer = 0f;
        }

        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }
    }*/

    void SpawnElement()
    {
        Vector3 spawnPos = new(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(0f, spawnHeight), 25f);
        GameObject toSpawn = Random.value > 0.7f ? airCurrent : obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(toSpawn, spawnPos, Quaternion.identity);
    }

    public void Activate()
    {
        //isActive = true;
        //timer = 0f;
    }
}
