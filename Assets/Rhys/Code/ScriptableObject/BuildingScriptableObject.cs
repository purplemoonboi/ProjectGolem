using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Builder = 0,
    Weapons = 1,
    Barricade = 2,
    Camp = 3,
    Turret = 4
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BuildingScriptableObject", order = 1)]
public class BuildingScriptableObject : ScriptableObject
{
    public string prefabName;
    public float health = 1000.0f;
    public float maximumHealth = 1000.0f;
    public int cost = 100;
    public int costToUpgrade = 150;
    public bool isActive = false;
    public bool isSpawning = false;
    public bool isMaxLevel = false;
    public int level = 1;
    public int maxLevel = 3;
    public BuildingType buildingType;
}


