using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainMesh))]
public class CustomMeshInspector : Editor
{

    private TerrainMesh terrainMesh;

    static int maxSize = 256;

    private static int size = 2;

    private float amplitude = 50.0f;
    private float frequency = 0.01f;
    private float offsetU = 0f;
    private float offsetV = 0f;
    private float lacunarity = 2f;
    private float loss = 0.5f;
    private int octaves = 8;

    // @brief Draw widgets to the scene view.
    private void OnSceneGUI()
    {

    }

    // @brief Create tools in the inspector.
    public override void OnInspectorGUI()
    {
        terrainMesh = target as TerrainMesh;

        if (GUILayout.Button("Generate Terrain"))
        {
            Undo.RecordObject(terrainMesh, "Generate Terrain");
            terrainMesh.GenerateMesh();
            EditorUtility.SetDirty(terrainMesh);
        }

        if (GUILayout.Button("Noise"))
        {
            Undo.RecordObject(terrainMesh, "Noise");
            terrainMesh.GenerateMesh();
            EditorUtility.SetDirty(terrainMesh);
        }

        EditorGUI.BeginChangeCheck();
        size = EditorGUILayout.IntSlider("Terrain Size", size, 2, maxSize);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Terrain Size");
            terrainMesh.SetTerrainSize(size);
            EditorUtility.SetDirty(terrainMesh);
        }

        EditorGUI.BeginChangeCheck();
        amplitude = EditorGUILayout.Slider("Amplitude", amplitude, 1, 10000.0f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Amplitude");
            terrainMesh.SetAmplitude(amplitude);
            EditorUtility.SetDirty(terrainMesh);
        }

        EditorGUI.BeginChangeCheck();
        frequency = EditorGUILayout.Slider("Frequency", frequency, 0.0001f, 0.99f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Frequency");
            terrainMesh.SetFrequency(frequency);
            EditorUtility.SetDirty(terrainMesh);
        }

        EditorGUI.BeginChangeCheck();
        loss = EditorGUILayout.Slider("Loss", loss, 0.001f, 0.99f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Loss");
            terrainMesh.SetLoss(loss)
            EditorUtility.SetDirty(terrainMesh);
        }

        EditorGUI.BeginChangeCheck();
        lacunarity = EditorGUILayout.Slider("Lacunarity", lacunarity, 1.0f, 10.0f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Lacunarity");
            EditorUtility.SetDirty(terrainMesh);
            terrainMesh.SetFrequency(lacunarity);
        }

        EditorGUI.BeginChangeCheck();
        offsetU = EditorGUILayout.Slider("Offset X", offsetU, 1.0f, 10000f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Offset X");
            EditorUtility.SetDirty(terrainMesh);
            terrainMesh.SetOffsetU(offsetU);
        }

        EditorGUI.BeginChangeCheck();
        offsetV = EditorGUILayout.Slider("Offset Z", offsetV, 1.0f, 10000f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(terrainMesh, "Offset Z");
            EditorUtility.SetDirty(terrainMesh);
            terrainMesh.SetOffsetV(offsetV);
        }
    }
}
