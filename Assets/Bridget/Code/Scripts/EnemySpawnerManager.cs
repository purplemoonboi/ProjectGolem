using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private float elapsedWaveTime;  //Elapsed time since last check if the next wave should begin
    private float elapsedClockTime;  //Elapsed time since we last checked if it's day or night

    [SerializeField] private bool isNighttime = false;   //To check if the game is currently in day or night mode
    [SerializeField] private TimeController dayNightCycle;
    [SerializeField] private EnemySpawnerScriptableObject spawnerSO;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<EnemySpawner> spawners;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<int> maxWavesPerNight = new List<int>();
    [SerializeField] private List<int[]> spawnCounts = new List<int[]>();
    [SerializeField] private List<int[]> enemyLimits = new List<int[]>();

    void Start()
    {
        SetupTimescales();
        SetupSpawners();
        SetupEnemyCounts();

        spawnerSO.nightsCompleted = 0;
        spawnerSO.wavesCompleted = 0;
    }

    void Update()
    {
        CheckTime();

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

    private void SetupEnemyCounts()
    {
        maxWavesPerNight.Add(3);
        maxWavesPerNight.Add(4);
        spawnerSO.maxWaves = maxWavesPerNight[0];

        spawnCounts.Add(new int[3]{ 1, 1, 2 });
        spawnCounts.Add(new int[4]{ 2, 2, 2, 3 });
        spawnerSO.spawnCount = spawnCounts[0][0];

        enemyLimits.Add(new int[3]{ 2, 3, 4 });
        enemyLimits.Add(new int[4]{ 3, 4, 5, 6 });
        spawnerSO.enemyLimit = enemyLimits[0][0];
    }

    private void SetupTimescales()
    {
        TimeSpan startTime = dayNightCycle.GetCurrentTime().TimeOfDay; //The time at launch
    
        if (!(startTime > dayNightCycle.GetSunrise() && startTime < dayNightCycle.GetSunset()))
            isNighttime = true;
        else
            isNighttime = false;
    }

    private void CheckTime()
    {
        elapsedClockTime += Time.deltaTime;

        if (elapsedClockTime > 1.0f)
        {
            elapsedClockTime = 0.0f;

            TimeSpan currentTime = dayNightCycle.GetCurrentTime().TimeOfDay;
            Debug.Log("Sunset: " + dayNightCycle.GetSunset().ToString() + " Sunrise: " + dayNightCycle.GetSunrise().ToString() + " Current Time" + currentTime.ToString());

            if (!isNighttime)    //If it's daytime
            {
                if (!(currentTime > dayNightCycle.GetSunrise() && currentTime < dayNightCycle.GetSunset()))
                    isNighttime = true;

                if (spawnerSO.wavesCompleted >= spawnerSO.maxWaves)
                {
                    if (spawnerSO.nightsCompleted < spawnerSO.maxNights)
                    {
                        spawnerSO.nightsCompleted++;
                        spawnerSO.wavesCompleted = 0;

                        if (spawnerSO.nightsCompleted < spawnerSO.maxNights)
                            spawnerSO.maxWaves = maxWavesPerNight[spawnerSO.nightsCompleted];
                    }
                }
            }
            else               //If it's nighttime
            {
                if ((currentTime > dayNightCycle.GetSunrise() && currentTime < dayNightCycle.GetSunset()))
                    isNighttime = false;
            }
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
            elapsedWaveTime = 0.0f;

            if (spawnerSO.nightsCompleted < spawnerSO.maxNights)
            {
                if (spawnerSO.wavesCompleted < spawnerSO.maxWaves)
                {
                    int enemiesRemaining = 0;

                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy != null)
                            enemiesRemaining++;
                    }

                    if (enemiesRemaining == 0)
                    {
                        ResetSpawners();

                        spawnerSO.wavesCompleted++;

                        //Updating how many enemies to spawn for the next wave after checking the previous wave wasn't the last one
                        if (spawnerSO.wavesCompleted < spawnerSO.maxWaves)
                        {
                            spawnerSO.spawnCount = spawnCounts[spawnerSO.nightsCompleted][spawnerSO.wavesCompleted];
                            spawnerSO.enemyLimit = enemyLimits[spawnerSO.nightsCompleted][spawnerSO.wavesCompleted];
                        }
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

    public int GetNightsCompleted() { return spawnerSO.nightsCompleted; }

    public int GetMaxNights() { return spawnerSO.maxNights; }

    public int GetWavesCompleted() { return spawnerSO.wavesCompleted; }

    public int GetMaxWaves() { return spawnerSO.maxWaves; }

    public float GetSpawnTime() { return spawnerSO.spawnTime; }

    public int GetSpawnCount() { return spawnerSO.spawnCount; }

    public int GetEnemyLimit() { return spawnerSO.enemyLimit; }
}