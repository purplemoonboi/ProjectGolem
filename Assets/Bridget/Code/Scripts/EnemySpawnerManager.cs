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
    private List<EnemySpawner> spawners;

    void Start()
    {
        SetupSpawners();
        spawnerSO.wavesCompleted = 0;
    }

    void Update()
    {
        CheckSpawners();
    }

    private void SetupSpawners()
    {
        var children = FindObjectsOfType<EnemySpawner>();

        foreach (var child in children)
        {
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

            int activeSpawners = 0;

            foreach(EnemySpawner spawner in spawners)
            {
                if (!spawner.GetFinishedWave())
                    activeSpawners++;
            }

            if(activeSpawners == 0 && spawnerSO.wavesCompleted < spawnerSO.maxWaves)
            {
                spawnerSO.wavesCompleted++;

                foreach (EnemySpawner spawner in spawners)
                    spawner.SetFinishedWave(false);
            }
        }
    }

    public int GetWavesCompleted() { return spawnerSO.wavesCompleted; }

    public int GetMaxWaves() { return spawnerSO.maxWaves; }
}