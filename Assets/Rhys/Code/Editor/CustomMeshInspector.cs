using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


[CustomEditor(typeof(TerrainMesh))]
public class CustomMeshInspector : Editor
{

    private TerrainMesh terrainMesh;

    static int maxSize = 1024;

    //Height map
    private int resolution = 2;
    private float amplitude = 50.0f;
    private float frequency = 0.01f;
    private float offsetU = 0f;
    private float offsetV = 0f;
    private float lacunarity = 2f;
    private float persistence = 0.5f;

    //Erosion
    private int seed = 2;
    private int erosionRadius = 2;
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
    private int cycles = 70000;

    private List<float> heightMap;
    private int mapSize = 1;

    // @brief Draw widgets to the scene view.
    private void OnSceneGUI()
    {

    }

    // @brief Create tools in the inspector.
    public override void OnInspectorGUI()
    {
        terrainMesh = target as TerrainMesh;

        EditorGUI.BeginChangeCheck();
        resolution = terrainMesh.GetResolution();
        resolution = EditorGUILayout.IntSlider("Base Resolution", resolution, 2, maxSize);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Base Resolution");
            terrainMesh.SetResolution(resolution);
            EditorUtility.SetDirty(terrainMesh);
        }

        EditorGUI.BeginChangeCheck();
        int scale = terrainMesh.GetScale();
        scale = EditorGUILayout.IntSlider("Terrain Scale", scale, 1, 1000);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Terrain Scale");
            terrainMesh.SetScale(scale);
            EditorUtility.SetDirty(terrainMesh);
        }

        EditorGUI.BeginChangeCheck();
        amplitude = terrainMesh.GetAmplitude();
        amplitude = EditorGUILayout.Slider("Amplitude", amplitude, 1.0f, 1000.0f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Amplitude");
            terrainMesh.SetAmplitude(amplitude);
            EditorUtility.SetDirty(terrainMesh);
        }
       
        EditorGUI.BeginChangeCheck();
        frequency = terrainMesh.GetFrequency();
        frequency = EditorGUILayout.Slider("Frequency", frequency, 0.001f, 0.99f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Frequency");
            terrainMesh.SetFrequency(frequency);
            EditorUtility.SetDirty(terrainMesh);
        }
       
        EditorGUI.BeginChangeCheck();
        persistence = terrainMesh.GetPersistence();
        persistence = EditorGUILayout.Slider("Persistence", persistence, 0.001f, 0.99f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Persistence");
            terrainMesh.SetPersistence(persistence);
            EditorUtility.SetDirty(terrainMesh);
        }
       
        EditorGUI.BeginChangeCheck();
        lacunarity = terrainMesh.GetLacunarity();
        lacunarity = EditorGUILayout.Slider("Lacunarity", lacunarity, 1.0f, 10.0f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Lacunarity");
            terrainMesh.SetLacunarity(lacunarity);
            EditorUtility.SetDirty(terrainMesh);
        }
       
        EditorGUI.BeginChangeCheck();
        offsetU = terrainMesh.GetOffsetU();
        offsetU = EditorGUILayout.Slider("Offset X", offsetU, 1.0f, 100f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Offset X");
            terrainMesh.SetOffsetU(offsetU);
            EditorUtility.SetDirty(terrainMesh);
        }
       
        EditorGUI.BeginChangeCheck();
        offsetV = terrainMesh.GetOffsetV();
        offsetV = EditorGUILayout.Slider("Offset Z", offsetV, 1.0f, 100f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Offset Z");
            terrainMesh.SetOffsetV(offsetV);
            EditorUtility.SetDirty(terrainMesh);
        }
       
        GUILayout.Space(4f);
       
        if (GUILayout.Button("Generate Terrain"))
        {
            Undo.RecordObject(terrainMesh, "Generate Terrain");
            terrainMesh.GenerateMesh();
            EditorUtility.SetDirty(terrainMesh);
        }

        GUILayout.Space(4f);

        if (GUILayout.Button("Smooth"))
        {
            Undo.RecordObject(terrainMesh, "Smooth");
            terrainMesh.Smooth();
            terrainMesh.ReloadMesh();
            EditorUtility.SetDirty(terrainMesh);
        }

        if (GUILayout.Button("Invert"))
        {
            Undo.RecordObject(terrainMesh, "Invert");
            terrainMesh.Invert();
            EditorUtility.SetDirty(terrainMesh);
        }


        GUILayout.Space(4f);

        //Erosion Tools


        heightMap = terrainMesh.GetHeightMap();
      
      
        EditorGUI.BeginChangeCheck();
        seed = terrainMesh.GetSeed();
        seed = EditorGUILayout.IntSlider("Seed", seed, 1, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Seed");
            terrainMesh.SetSeed(seed);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        erosionRadius = terrainMesh.GetErosionRadius();
        erosionRadius = EditorGUILayout.IntSlider("Erosion Radius", erosionRadius, 1, 32);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Erosion Radius");
            terrainMesh.SetErosionRadius(erosionRadius);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        particleIntertia = terrainMesh.GetParticleIntertia();
        particleIntertia = EditorGUILayout.Slider("Particle Inertia", particleIntertia, 0.01f, 1.0f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Particle Inertia");
            terrainMesh.SetParticleIntertia(particleIntertia);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        sedimentCapacityFactor = terrainMesh.GetSedimentCapacityFactor();
        sedimentCapacityFactor = EditorGUILayout.Slider("Sediment Capacity Factor", sedimentCapacityFactor, 1, 32);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Sediment Capacity Factor");
            terrainMesh.SetSedimentCapacityFactor(sedimentCapacityFactor);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        minimumSlope = terrainMesh.GetMinimumSlope();
        minimumSlope = EditorGUILayout.Slider("Minimum Slope", minimumSlope, 0.01f, 0.9f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Minimum Slope");
            terrainMesh.SetMinimumSlope(minimumSlope);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        erosionFactor = terrainMesh.GetErosionFactor();
        erosionFactor = EditorGUILayout.Slider("Erosion Factor", erosionFactor, 0.01f, 10f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Erosion Factor");
            terrainMesh.SetErosionFactor(erosionFactor);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        depositionFactor = terrainMesh.GetDepositionFactor();
        depositionFactor = EditorGUILayout.Slider("Deposition Factor", depositionFactor, 0.01f, 1f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Deposition Factor");
            terrainMesh.SetDepositionFactor(depositionFactor);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        evaporationSpeed = terrainMesh.GetEvaporationSpeed();
        evaporationSpeed = EditorGUILayout.Slider("Evaporation Speed", evaporationSpeed, 0.01f, 0.9f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Evaporation Speed");
            terrainMesh.SetEvaporationSpeed(evaporationSpeed);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        gravity = terrainMesh.GetGravity();
        gravity = EditorGUILayout.IntSlider("Gravity", gravity, 2, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Gravity");
            terrainMesh.SetGravity(gravity);
            EditorUtility.SetDirty(terrainMesh);
        }
      
        EditorGUI.BeginChangeCheck();
        particleLifetime = terrainMesh.GetParicleLifetime();
        particleLifetime = EditorGUILayout.IntSlider("Particle Lifetime", particleLifetime, 2, 100);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Particle Lifetime");
            terrainMesh.SetParicleLifetime(particleLifetime);
            EditorUtility.SetDirty(terrainMesh);
        }

        EditorGUI.BeginChangeCheck();
        int iterations = EditorGUILayout.IntSlider("Iterations", cycles, 1, 500000);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Iterations");
            cycles = iterations;
            EditorUtility.SetDirty(terrainMesh);
        }

        GUILayout.Space(4f);
      
        if (GUILayout.Button("Apply Erosion"))
        {
            Undo.RecordObject(terrainMesh, "Apply Erosion");
            terrainMesh.Erode(heightMap.ToArray(), resolution, cycles, true);
            terrainMesh.ReloadMesh();
            EditorUtility.SetDirty(terrainMesh);
        }

        GUILayout.Space(4f);

        if (GUILayout.Button("Bake Height Map"))
        {
            terrainMesh.BakeHeightMap();
        }

        GUILayout.Space(4f);

        string prefix = Directory.GetCurrentDirectory();
        string fileName = "/HeightMaps/HeightMap.png";
        string filePath = prefix + fileName;
        GUILayout.Space(4f);

        if (GUILayout.Button("Load From File"))
        {
            terrainMesh.LoadFromFile(filePath);
        }

    }
}
