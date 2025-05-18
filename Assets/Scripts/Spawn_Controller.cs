using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Controller : MonoBehaviour
{
    /*    public GameObject[] obstacles;
        public GameObject airCurrent;
        public float spawnRate = 2f;
        public float spawnRangeX = 3f;
        public float spawnHeight = 5f;
        public float spawnInterval = 2f;*/

    [Header("Obstacle Categories")]
    public GameObject[] groundObstacles;
    public GameObject[] airObstacles;

    [Header("Air Current")]
    public GameObject airCurrent;

    [Header("Spawn Settings")]
    public float spawnRangeX = 5f;
    public float groundMinY = 0.5f;
    public float groundMaxY = 1.5f;
    public float airMinY = 2.5f;
    public float airMaxY = 5f;
    public float spawnZ = 25f;

    [Header("Spawn Timing")]
    public float initialSpawnInterval = 2f;   // Comienza cada 2 segundos
    public float minSpawnInterval = 0.6f;     // No baja de 0.6s
    public float spawnAcceleration = 0.05f;   // Cuánto se reduce por ciclo
    public float difficultyInterval = 10f;    // Cada cuántos segundos aumenta dificultad

    private float currentSpawnInterval;
    private float nextDifficultyTime;

    //private bool isActive = false;
    //private float timer;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        nextDifficultyTime = Time.time + difficultyInterval;
        StartCoroutine(SpawnRoutine());
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
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnElement();
            yield return new WaitForSeconds(currentSpawnInterval);

            // Aumentar dificultad con el tiempo
            if (Time.time >= nextDifficultyTime && currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= spawnAcceleration;
                nextDifficultyTime = Time.time + difficultyInterval;
            }
        }
    }
    void SpawnElement()
    {
        /*        Vector3 spawnPos = new(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(0f, spawnHeight), 25f);
                GameObject toSpawn = Random.value > 0.7f ? airCurrent : obstacles[Random.Range(0, obstacles.Length)];
                Instantiate(toSpawn, spawnPos, Quaternion.identity);*/
        GameObject toSpawn;
        Vector3 spawnPos;

        // Elegir qué objeto spawnear: corriente o obstáculo
        if (Random.value > 0.7f)
        {
            // Corriente de aire: posición baja
            toSpawn = airCurrent;
            float yPos = Random.Range(groundMinY, groundMaxY); // cerca del suelo
            spawnPos = new(Random.Range(-spawnRangeX, spawnRangeX), yPos, spawnZ);
            //Instantiate(airCurrent, spawnPos, Quaternion.identity);
        }
        else
        {
            // Spawn obstáculo (50% chance de aéreo o suelo)
            bool spawnAir = Random.value > 0.5f;

            if (spawnAir && airObstacles.Length > 0)
            {
                toSpawn = airObstacles[Random.Range(0, airObstacles.Length)];
                float yPos = Random.Range(airMinY, airMaxY);
                spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), yPos, spawnZ);
            }
            else if (groundObstacles.Length > 0)
            {
                toSpawn = groundObstacles[Random.Range(0, groundObstacles.Length)];
                float yPos = Random.Range(groundMinY, groundMaxY);
                spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), yPos, spawnZ);
            }
            else
            {
                return; // No hay obstáculos disponibles, no instanciar nada
            }
        }

        Instantiate(toSpawn, spawnPos, Quaternion.identity);
    }

    public void Activate()
    {
        //isActive = true;
        //timer = 0f;
    }
}
