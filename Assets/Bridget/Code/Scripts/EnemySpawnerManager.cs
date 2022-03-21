using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private float elapsedWaveTime;  //Elapsed time since last check if the next wave should begin

    [SerializeField] private EnemySpawnerScriptableObject spawnerSO;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<EnemySpawner> spawners;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private int[] spawnCountPerWave = new int[3] { 1, 1, 2 };
    [SerializeField] private int[] enemyLimitsPerWave = new int[3] { 2, 3, 4 };

    void Start()
    {
        SetupSpawners();

        spawnerSO.wavesCompleted = 0;
        spawnerSO.spawnCount = spawnCountPerWave[0];
        spawnerSO.enemyLimit = enemyLimitsPerWave[0];
    }

    void Update()
    {
        if(enemies.Count > 0)
        CheckSpawners();
    }

    private void SetupSpawners()
    {
        var children = FindObjectsOfType<EnemySpawner>();

        foreach (var child in children)
        {
            child.SetEnemyPrefab(enemyPrefab);
            spawners.Add(child);
        }
    }

    //@brief
    //Iterates over all active enemy spawners every 3 seconds, checking how many enemies are still alive from the current wave.
    //If all enemies have been killed, the wave is completed and the next one begins (assuming it's nighttime) until the maximum is reached.
    private void CheckSpawners()
    {
        elapsedWaveTime += Time.deltaTime;

        if(elapsedWaveTime > 3.0f)
        {
            Debug.Log("Checking enemy spawners...");
            elapsedWaveTime = 0.0f;

            if(spawnerSO.wavesCompleted < spawnerSO.maxWaves)
            {
                int enemiesRemaining = 0;

                foreach(GameObject enemy in enemies)
                {
                    if(enemy != null)
                    enemiesRemaining++;
                }

                Debug.Log("Enemies Remaining: " + enemiesRemaining);

                if (enemiesRemaining == 0)
                {
                    ResetSpawners();

                    spawnerSO.wavesCompleted++;

                    //Updating how many enemies to spawn for the next wave after checking the previous wave wasn't the last one
                    if (spawnerSO.wavesCompleted < spawnerSO.maxWaves)
                    {
                        spawnerSO.spawnCount = spawnCountPerWave[spawnerSO.wavesCompleted];
                        spawnerSO.enemyLimit = enemyLimitsPerWave[spawnerSO.wavesCompleted];
                    }
                }
            }
        }
    }

    private void ResetSpawners()
    {
        enemies.Clear();

        foreach (EnemySpawner spawner in spawners)
        {
            spawner.SetSpawnedEnemies(0);
        }
    }

    public void AddSpawnedEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public int GetWavesCompleted() { return spawnerSO.wavesCompleted; }

    public int GetMaxWaves() { return spawnerSO.maxWaves; }

    public float GetSpawnTime() { return spawnerSO.spawnTime; }

    public int GetSpawnCount() { return spawnerSO.spawnCount; }

    public int GetEnemyLimit() { return spawnerSO.enemyLimit; }
}