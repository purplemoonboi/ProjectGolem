using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    public string prefabName;
    public float health = 100.0f;
    public float damageOutput = 50.0f;
    public int level = 1;
}

