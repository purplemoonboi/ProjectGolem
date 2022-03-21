using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "Scriptable Objects/Enemy Spawner Scriptable Object", order = 1)]
public class EnemySpawnerScriptableObject : ScriptableObject
{
    public int maxNights = 2;   //How many nights will pass in-game
    public int nightsCompleted = 0; //The number of nights that have passed thus far
    public int maxWaves = 3;    //The number of waves to generate before disabling the spawner
    public int wavesCompleted = 0;  //The number of waves completed by the player
    public float spawnTime = 1.0f;    //Time in seconds between enemy spawns
    public int spawnCount = 1;     //The number of enemies to spawn in one go
    public int enemyLimit = 2;     //The maximum number of enemies for the spawner to create per wave
}