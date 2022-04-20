using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FriendlyData", menuName = "Scriptable Objects/Friendly Scriptable Object", order = 1)]
public class FriendlyScriptableObject : ScriptableObject
{
    public float MAX_HEALTH = 1000.0f;
    public float power = 20.0f;
    public float repairRate = 20.0f;
}