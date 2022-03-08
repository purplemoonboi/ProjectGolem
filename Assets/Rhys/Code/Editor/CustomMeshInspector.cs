using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainMesh))]
public class CustomMeshInspector : Editor
{

    private TerrainMesh terrainMesh;
    private int size = 1;

    static int maxSize = 1000;
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

        EditorGUILayout.IntSlider("Terrain Size", size, 1, maxSize);
        terrainMesh.SetTerrainSize(size);
    }
}
