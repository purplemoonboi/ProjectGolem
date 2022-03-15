using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private float elapsedWaveTime;  //Elapsed time since last check if the next wave should begin

    [SerializeField]
    private EnemySpawnerScriptableObject spawnerSO;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<EnemySpawner> spawners;
    [SerializeField]
    private List<GameObject> enemies;

    void Start()
    {
        SetupSpawners();
        
        spawnerSO.wavesCompleted = 0;
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
                    enemies.Clear();
                    spawnerSO.wavesCompleted++;
                }
            }
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