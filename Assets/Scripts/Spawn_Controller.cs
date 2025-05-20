using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Controller : MonoBehaviour
{
    private Game_Controller controller;

    [Header("Obstacle Categories")]
    public GameObject[] groundObstacles;
    public GameObject[] airObstacles;

    [Header("Air Current")]
    public GameObject airCurrent;

    [Header("Environment")]
    public GameObject[] environments_Objects;

    [Header("Buildings")]
    public GameObject[] buildingPrefabs;
    public float buildingSpacing = 3f;
    public int maxBuildingsOnScreen = 7;
    public float buildingScrollSpeed = 5f;

    private List<GameObject> activeBuildings = new();
    //private float lastBuildingZ = 0f;

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
    int counter = 0;

    void Start()
    {
        controller = FindAnyObjectByType<Game_Controller>();
        currentSpawnInterval = initialSpawnInterval;
        nextDifficultyTime = Time.time + difficultyInterval;
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        if (!controller.beforeStart)
        {
            SpawnBuildings();
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnElement();

            // Cada 2 ciclos, spawnea un objeto de entorno
            counter++;
            if (counter % 2 == 0)
                SpawnEnvironment();

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
        GameObject toSpawn;
        Vector3 spawnPos;

        // Elegir qué objeto spawnear: corriente o obstáculo
        if (Random.value > 0.7f)
        {
            // Corriente de aire: posición baja
            toSpawn = airCurrent;
            //float yPos = Random.Range(groundMinY, groundMaxY); // cerca del suelo
            spawnPos = new(Random.Range(-spawnRangeX, spawnRangeX), 0f, spawnZ);
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
                //float yPos = Random.Range(groundMinY, groundMaxY);
                spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0.5f, spawnZ);
            }
            else
            {
                return; // No hay obstáculos disponibles, no instanciar nada
            }
        }

        Instantiate(toSpawn, spawnPos, toSpawn.transform.rotation);
    }

    void SpawnEnvironment()
    {
        // Seleccionamos un objeto de entorno aleatoriamente
        GameObject envObject = environments_Objects[Random.Range(0, environments_Objects.Length)];
        Vector3 spawnPos = Vector3.zero;

/*        // Clasificamos por nombre, tag o incluso un sistema mejor si hay muchos
        if (envObject.tag.Contains("Building"))
        {
            spawnPos = new Vector3(-10f, 0f, spawnZ);
        }*/
        if (envObject.tag.Contains("Tree") || envObject.tag.Contains("Decoration"))
        {
            spawnPos = new Vector3(6f, 0.2f, spawnZ);
        }
        else if (envObject.tag.Contains("Car") || envObject.tag.Contains("Street"))
        {
            spawnPos = new Vector3(11, 0f, spawnZ);
        }

        Instantiate(envObject, spawnPos, Quaternion.identity);
    }

    void SpawnBuildings()
    {
        /*        // Verificar si el último edificio está lo suficientemente atrás para generar uno nuevo
                if (activeBuildings.Count == 0 || 
                    activeBuildings[activeBuildings.Count - 1].transform.position.z < (Camera.main.transform.position.z + 15f))
                {
                    GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                    Vector3 spawnPos = new(-10f, 0f, lastBuildingZ); // -10f en X para la izquierda de la pantalla
                    GameObject building = Instantiate(prefab, spawnPos, Quaternion.identity);
                    activeBuildings.Add(building);

                    lastBuildingZ += buildingSpacing; // Pegado sin espacio
                    //Debug.Log("Espacio" + lastBuildingZ);
                    lastBuildingZ = 0f;
                }*/

        // Si no hay edificios, instanciamos el primero en la posición inicial
        if (activeBuildings.Count == 0)
        {
            GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
            GameObject building = Instantiate(prefab, new Vector3(-10f, 0f, 0f), Quaternion.identity);
            activeBuildings.Add(building);
        }
        else
        {
            // Verificar si el último edificio está suficientemente atrás para instanciar otro
            GameObject last = activeBuildings[activeBuildings.Count - 1];

            // Obtener tamaño real del último edificio
            Renderer lastRenderer = last.GetComponent<Renderer>();
            float lastZ = last.transform.position.z;
            float lastLength = lastRenderer.bounds.size.z;

            // Solo instanciar si el último edificio ya está lo suficientemente cerca de la cámara
            if (lastZ < Camera.main.transform.position.z + 15f)
            {
                GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                GameObject building = Instantiate(prefab, Vector3.zero, Quaternion.identity);

                // Obtener tamaño del nuevo edificio
                Renderer newRenderer = building.GetComponent<Renderer>();
                float newLength = newRenderer.bounds.size.z;

                // Posicionar el nuevo edificio justo después del anterior
                float newZ = lastZ + (lastLength / 2f) + (newLength / 2f);
                building.transform.position = new Vector3(-10f, 0f, newZ);

                activeBuildings.Add(building);
            }
        }
    }
}
