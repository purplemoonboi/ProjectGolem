using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelPath))]
public class PathInspector : Editor
{

    private LevelPath path;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const int stepsPerCurve = 10;
    private float directionScale = 1.0f;
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;
    private int selectedIndex = -1;
    private bool showDirections = true;


    // @brief Draw widgets to the scene view.
    private void OnSceneGUI()
    {
        path = target as LevelPath;
        handleTransform = path.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Vector3 point0 = ShowPoint(0);

        for (int i = 1; i < path.ControlPointCount(); i += 3)
        {
            Vector3 point1 = ShowPoint(i);
            Vector3 point2 = ShowPoint(i + 1);
            Vector3 point3 = ShowPoint(i + 2);
          
            Handles.color = Color.blue;
            Handles.DrawLine(point0, point1);
            Handles.DrawLine(point2, point3);
          
            Handles.DrawBezier(point0, point3, point1, point2, Color.green, null, 6f);
            point0 = point3;
        }

        if(showDirections)
        {
            ShowDirections();
        }
    }

    // @brief Create tools in the inspector.
    public override void OnInspectorGUI()
    {
        path = target as LevelPath;

        EditorGUI.BeginChangeCheck();
        bool toggleDirections = EditorGUILayout.Toggle("Show Directions", showDirections);
        if (EditorGUI.EndChangeCheck())
        {
            showDirections = toggleDirections;
        }

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", path.Loop);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(path, "Toggle Loop");
            EditorUtility.SetDirty(path);
            path.Loop = loop;
        }
       
        if(selectedIndex >= 0 && selectedIndex < path.ControlPointCount())
        {
            DrawSelectedPointInspector();
        }
        if(GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(path, "Add Curve");
            path.AddCurve();
            EditorUtility.SetDirty(path);
        }

        GUILayout.Space(8f);

        GUILayout.Label("Reset Curve");
        if(GUILayout.Button("Reset"))
        {
            Undo.RecordObject(path, "Reset");
            path.Reset();
            EditorUtility.SetDirty(path);
        }

        if (GUILayout.Button("Carve Path"))
        {
            Undo.RecordObject(path, "Carve Path");
            path.CarveTerrain();
            EditorUtility.SetDirty(path);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", path.GetControlPoint(selectedIndex));
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(path, "Move Point");
            EditorUtility.SetDirty(path);
            path.SetControlPoint(selectedIndex, point);
        }

        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
        EditorGUILayout.EnumPopup("Mode", path.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(path, "Change Point Mode");
            path.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(path);
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.cyan;
        Vector3 point = path.GetPointOnSpline(0f);
        Handles.DrawLine(point, point + path.GetDirection(0f) * directionScale);

        int steps = stepsPerCurve * path.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = path.GetPointOnSpline(i / (float)steps);
            Handles.DrawLine(point, point + path.GetDirection(i / (float)steps) * directionScale);
        }
    }

    private static Color[] modeColours =
    {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private Vector3 ShowPoint(int index)
    {
         Vector3 point = handleTransform.TransformPoint(path.GetControlPoint(index));
        
         float size = HandleUtility.GetHandleSize(point);
         //Increase the size of the first control point as looping will cause overlap.
         if (index == 0)
         {
             size *= 2f;
         }
         Handles.color = modeColours[(int)path.GetControlPointMode(index)];
        
         if(Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
         {
             selectedIndex = index;
             Repaint();
         }
         if(selectedIndex == index)
         {
             EditorGUI.BeginChangeCheck();
             point = Handles.DoPositionHandle(point, handleRotation);
             if (EditorGUI.EndChangeCheck())
             {
                 //Tell Unity to record the last operation of the object.
                 //So we can undo.
                 Undo.RecordObject(path, "Move Point");
                 EditorUtility.SetDirty(path);
        
                 //Transform to local space.
                 path.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
             }
         }
        return point;
    }

}
