using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// @author Rhys Duff
// @brief  Grid tool kit to customise size and pattern of grid.

[CustomEditor(typeof(GridManager))]
public class BuilderInspectorTools : Editor
{
   [SerializeField]
   private int width = 10;
   [SerializeField]
   private int breadth = 10;
   [SerializeField]
   private int cellPadding = 0;
   [SerializeField]
   private Vector3 gridPosition = new Vector3();
   [SerializeField]
   private GameObject prefab;


    public override void OnInspectorGUI()
    {
        GridManager thisTarget = (GridManager)target;

        /*Create some tools for grid generation*/
        width        = EditorGUILayout.IntField("Grid Width",   width);
        breadth      = EditorGUILayout.IntField("Grid Breadth", breadth);
        cellPadding  = EditorGUILayout.IntField("Cell Padding", cellPadding);
        gridPosition = EditorGUILayout.Vector3Field("Grid Position", gridPosition);

        prefab           = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), true);

        /*Update the grid manager's parameters*/
        thisTarget.SetGridDimensions(width, breadth);
        thisTarget.SetCellPadding(cellPadding);
        thisTarget.SetGridPosition(gridPosition);

        /*A button for the user to build a grid in edit mode*/
        if(GUILayout.Button("Generate Level Grid"))
        {
            if(prefab != null)
            {
                thisTarget.GenerateNewGrid(width, breadth, prefab, gridPosition, cellPadding);
            }
        }
    }
}
