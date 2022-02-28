//@author Bridget Casey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnTime;    //Time in seconds between enemy spawns
    private float elapsedTime;
    [SerializeField]
    private int spawnCount;     //The number of enemies to spawn in one go
    private int enemyLimit;     //The maximum number of enemies to have in the scene at once
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<GameObject> enemies;
    [SerializeField]
    private bool shouldSpawn = false;

    void Start()
    {
        spawnTime = 3.0f;
        elapsedTime = 0.0f;
        spawnCount = 2;
        enemyLimit = 10;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            shouldSpawn = (shouldSpawn == false) ? true : false;
        }

        if(shouldSpawn)
        {
            SpawnOnTimer();
        }
    }

    private void SpawnOnTimer()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > spawnTime)
        {
            elapsedTime = 0.0f;

            for (int i = 0; i < spawnCount; i++)
            {
                if (enemies.Count >= enemyLimit)
                    break;

                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        //Sets enemies to spawn on top of the ground and a random distance from the spawner within a range of 5 units
        Vector3 spawnPoint = new Vector3(transform.position.x + Random.Range(-5.0f, 5.0f), enemyPrefab.transform.localScale.y, transform.position.z + Random.Range(-5.0f, 5.0f));

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        enemy.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), Random.Range(0.0f, 359.9f));
        enemy.name = "Enemy " + (enemies.Count + 1);
        //enemy.GetComponent<EnemyMovement>().SetID(enemies.Count + 1);

        enemies.Add(enemy);
    }
}
