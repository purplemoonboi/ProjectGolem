using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HydraulicErosion)), CanEditMultipleObjects]
public class HydraulicErosionInspector : Editor
{

    private TerrainMesh terrainMesh;
    private HydraulicErosion hydraulicErosion;

    private int seed = 2;
    private int erosionRadius;
    private float particleIntertia = 0.05f;
    private float sedimentCapacityFactor = 4;
    private float minimumSlope = 0.01f;
    private float erosionFactor = 0.3f;
    private float depositionFactor = 0.3f;
    private float evaporationSpeed = 0.01f;
    private int gravity;
    private int particleLifetime = 20;
    private const int waterVolume = 1;
    private float initialVelocity = 1;

    private List<float> heightMap;
    private int mapSize = 1;

    // @brief Create tools in the inspector.
    public override void OnInspectorGUI()
    {
        //First grab some data from the mesh.
        terrainMesh = target as TerrainMesh;
        heightMap = terrainMesh.GetHeightMap();
        mapSize = terrainMesh.GetTerrainDimensions();

        hydraulicErosion = target as HydraulicErosion;

        EditorGUI.BeginChangeCheck();
        seed = terrainMesh.GetResolution();
        seed = EditorGUILayout.IntSlider("Seed", seed, 1, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Seed");
            hydraulicErosion.SetSeed(seed);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        erosionRadius = terrainMesh.GetResolution();
        erosionRadius = EditorGUILayout.IntSlider("Erosion Radius", erosionRadius, 1, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Erosion Radius");
            hydraulicErosion.SetErosionRadius(erosionRadius);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        particleIntertia = terrainMesh.GetResolution();
        particleIntertia = EditorGUILayout.Slider("Particle Inertia", particleIntertia, 0.01f, 1.0f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Particle Inertia");
            hydraulicErosion.SetParticleIntertia(particleIntertia);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        sedimentCapacityFactor = terrainMesh.GetResolution();
        sedimentCapacityFactor = EditorGUILayout.Slider("Sediment Capacity Factor", sedimentCapacityFactor, 1, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Sediment Capacity Factor");
            hydraulicErosion.SetSedimentCapacityFactor(sedimentCapacityFactor);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        minimumSlope = terrainMesh.GetResolution();
        minimumSlope = EditorGUILayout.Slider("Minimum Slope", minimumSlope, 0.01f, 0.9f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Minimum Slope");
            hydraulicErosion.SetMinimumSlope(minimumSlope);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        erosionFactor = terrainMesh.GetResolution();
        erosionFactor = EditorGUILayout.Slider("Erosion Factor", erosionFactor, 0.01f, 0.9f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Erosion Factor");
            hydraulicErosion.SetErosionFactor(erosionFactor);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        depositionFactor = terrainMesh.GetResolution();
        depositionFactor = EditorGUILayout.Slider("Deposition Factor", depositionFactor, 0.01f, 0.9f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Deposition Factor");
            hydraulicErosion.SetDepositionFactor(depositionFactor);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        evaporationSpeed = terrainMesh.GetResolution();
        evaporationSpeed = EditorGUILayout.Slider("Evaporation Speed", evaporationSpeed, 0.01f, 0.9f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Evaporation Speed");
            hydraulicErosion.SetEvaporationSpeed(evaporationSpeed);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        gravity = terrainMesh.GetResolution();
        gravity = EditorGUILayout.IntSlider("Gravity", gravity, 2, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Gravity");
            hydraulicErosion.SetGravity(gravity);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        EditorGUI.BeginChangeCheck();
        particleLifetime = terrainMesh.GetResolution();
        particleLifetime = EditorGUILayout.IntSlider("Particle Lifetime", particleLifetime, 2, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Particle Lifetime");
            hydraulicErosion.SetGravity(particleLifetime);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        GUILayout.Space(4f);

        if (GUILayout.Button("Apply Erosion"))
        {
            Undo.RecordObject(hydraulicErosion, "Apply Erosion");
            hydraulicErosion.Erode(heightMap.ToArray(), mapSize, 1, true);
            EditorUtility.SetDirty(hydraulicErosion);
        }

        terrainMesh = target as TerrainMesh;

        terrainMesh.UpdateHeightMap(heightMap);

    }

}
