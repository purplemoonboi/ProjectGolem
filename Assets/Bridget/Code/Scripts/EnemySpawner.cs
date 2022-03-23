//@author Bridget Casey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float elapsedSpawnTime; //Elapsed time since last enemy was spawned

    private EnemySpawnerManager spawnManager;
    private GameObject enemyPrefab;

    [SerializeField] private int spawnedEnemies = 0;
    [SerializeField] private bool shouldSpawn = false;
    [SerializeField] private TimeController dayNightCycle;

    void Start()
    {
        elapsedSpawnTime = 0.0f;

        spawnManager = FindObjectOfType<EnemySpawnerManager>();
    }

    void Update()
    {

        if (spawnManager == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
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

        if (spawnManager.GetNightsCompleted() < spawnManager.GetMaxNights())
        {
            if (spawnManager.GetWavesCompleted() < spawnManager.GetMaxWaves())
            {
                if (shouldSpawn)
                {
                    SpawnOnTimer();
                }
            }
        }
    }

    private void SpawnOnTimer()
    {
        elapsedSpawnTime += Time.deltaTime;

        if(elapsedSpawnTime > spawnManager.GetSpawnTime())
        {
            elapsedSpawnTime = 0.0f;

            for (int i = 0; i < spawnManager.GetSpawnCount(); i++)
            {
                if (spawnedEnemies >= spawnManager.GetEnemyLimit())
                    break;

                SpawnEnemy();
                spawnedEnemies++;
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

        spawnManager.AddSpawnedEnemy(enemy);
    }

    public void SetEnemyPrefab(GameObject enemy) { enemyPrefab = enemy; }

    public void SetSpawnedEnemies(int enemies) { spawnedEnemies = enemies; }
}