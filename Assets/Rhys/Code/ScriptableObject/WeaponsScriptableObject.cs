using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponsScriptableObject", order = 1)]
public class WeaponsScriptableObject : ScriptableObject
{
    public string prefabName;
    public float maximumHealth = 1000.0f;
    public bool isMaxLevel = false;
    public int level = 1;
    public int maxLevel = 3;
}
