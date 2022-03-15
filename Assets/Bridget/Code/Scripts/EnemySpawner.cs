//@author Bridget Casey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float elapsedSpawnTime; //Elapsed time since last enemy was spawned
    private float elapsedDespawnTime;   //Elapsed time since last check was performed if enemies should despawn

    private EnemySpawnerManager spawnManager;

    [SerializeField]
    private float spawnTime;    //Time in seconds between enemy spawns
    [SerializeField]
    private int spawnCount;     //The number of enemies to spawn in one go
    [SerializeField]
    private int enemyLimit;     //The maximum number of enemies for the spawner to create per wave
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<GameObject> enemies;
    [SerializeField]
    private bool shouldSpawn = false;
    [SerializeField]
    bool finishedWave = false;  //A flag to check if this spawner has finished the current wave and should progress
    [SerializeField]
    private TimeController dayNightCycle;

    void Start()
    {
        elapsedSpawnTime = 0.0f;
        elapsedDespawnTime = 0.0f;

        spawnManager = FindObjectOfType<EnemySpawnerManager>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            shouldSpawn = (shouldSpawn == false) ? true : false;
        }

        if(!(dayNightCycle.GetCurrentTime().TimeOfDay > dayNightCycle.GetSunrise()
            &&
            dayNightCycle.GetCurrentTime().TimeOfDay < dayNightCycle.GetSunset()))
        {
            shouldSpawn = true;
        }
        else
        {
            shouldSpawn = false;
        }

        if (spawnManager.GetWavesCompleted() < spawnManager.GetMaxWaves())
        {
            if (shouldSpawn)
            {
                SpawnOnTimer();
            }

            CheckActiveWave();
        }
    }

    private void SpawnOnTimer()
    {
        elapsedSpawnTime += Time.deltaTime;

        if(elapsedSpawnTime > spawnTime)
        {
            elapsedSpawnTime = 0.0f;

            for (int i = 0; i < spawnCount; i++)
            {
                if (enemies.Count >= enemyLimit)
                    break;

                SpawnEnemy();
            }
        }
    }

    //@brief
    //Regularly checks the currently active enemy wave (by default, every 3 seconds) for living enemies.
    //If none are still alive, then progress to the next wave.
    private void CheckActiveWave()
    {
        if (enemies.Count > 0)
        {
            elapsedDespawnTime += Time.deltaTime;

            if (elapsedDespawnTime > 3.0f)
            {
                Debug.Log("Checking active wave for " + gameObject.name + "...");
                elapsedDespawnTime = 0.0f;

                int activeEnemies = 0;

                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyController>().GetAlive())
                        activeEnemies++;
                }

                if (activeEnemies == 0)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        Destroy(enemy);
                    }

                    enemies.Clear();
                    finishedWave = true;
                }
            }
        }
    }

    //@brief
    //Spawns a single instance of an enemy at the spawner's world position.
    private void SpawnEnemy()
    {
        //Sets enemies to spawn on top of the ground and a random distance from the spawner within a range of 5 units
        Vector3 spawnPoint = new Vector3(transform.position.x + Random.Range(-5.0f, 5.0f), transform.position.y, transform.position.z + Random.Range(-5.0f, 5.0f));

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        enemy.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), Random.Range(0.0f, 359.9f));
        enemy.name = "Enemy " + (enemies.Count + 1);

        enemies.Add(enemy);
    }

    public void SetFinishedWave(bool wave) { finishedWave = wave; }

    public bool GetFinishedWave() { return finishedWave; }
}