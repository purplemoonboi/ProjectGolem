using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker 
{
    [SerializeField]
    private static NavMeshBuildSettings navMeshBuildSettings;
    [SerializeField]
    private static List<NavMeshBuildSource> navMeshSources;
    [SerializeField]
    private static Bounds navBounds;
    [SerializeField]
    private static Vector3 position;
    [SerializeField]
    private static Quaternion rotation;

    // @brief Rebakes nav mesh at runtime.
    public static void BakeNavMesh()
    {
        NavMeshBuilder.BuildNavMeshData(navMeshBuildSettings, navMeshSources, navBounds, position, rotation);
    }
}
