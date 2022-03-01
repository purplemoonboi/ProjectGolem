using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TurretScriptableObject", order = 1)]
public class TurretScriptableObject : ScriptableObject
{
    public string prefabName;
    public float maximumHealth = 1000.0f;
    public bool isMaxLevel = false;
    public int cost = 0;
    public int costToUpgrade = 0;
    public int level = 1;
    public int maxLevel = 3;
}