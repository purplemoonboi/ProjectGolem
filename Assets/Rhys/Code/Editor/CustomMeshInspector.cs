using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainMesh))]
public class CustomMeshInspector : Editor
{

    private TerrainMesh terrainMesh;


    static int maxSize = 256;

    private int resolution = 2;
    private float amplitude = 50.0f;
    private float frequency = 0.01f;
    private float offsetU = 0f;
    private float offsetV = 0f;
    private float lacunarity = 2f;
    private float loss = 0.5f;

    private const int octaves = 8;

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
        amplitude = terrainMesh.GetAmplitude();
        amplitude = EditorGUILayout.Slider("Amplitude", amplitude, 1, 100.0f);
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
        loss = terrainMesh.GetLoss();
        loss = EditorGUILayout.Slider("Loss", loss, 0.001f, 0.99f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Loss");
            terrainMesh.SetLoss(loss);
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

        if (GUILayout.Button("Bake Height Map"))
        {
            terrainMesh.BakeHeightMap();
        }

   
    }
}
