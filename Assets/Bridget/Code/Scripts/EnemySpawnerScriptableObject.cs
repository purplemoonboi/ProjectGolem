using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "Scriptable Objects/Enemy Spawner Scriptable Object", order = 1)]
public class EnemySpawnerScriptableObject : ScriptableObject
{
    public int maxWaves = 3;    //The number of waves to generate before disabling the spawner
    public int wavesCompleted = 0;  //The number of waves completed by the player
    public float spawnTime;    //Time in seconds between enemy spawns
    public int spawnCount;     //The number of enemies to spawn in one go
    public int enemyLimit;     //The maximum number of enemies for the spawner to create per wave
}