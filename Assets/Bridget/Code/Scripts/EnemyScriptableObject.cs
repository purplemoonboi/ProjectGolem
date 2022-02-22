using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Enemy Scriptable Object", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    public float MAX_HEALTH = 1000.0f;
    public float power = 10.0f;
}
