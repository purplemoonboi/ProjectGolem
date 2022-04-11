using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spline))]
public class SplineInspector : Editor
{

    private Spline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const int stepsPerCurve = 10;
    private float directionScale = 1.0f;
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;
    private int selectedIndex = -1;
    private bool showDirections = true;
    private bool realTimeEditing = false;

    // @brief Draw widgets to the scene view.
    private void OnSceneGUI()
    {
        spline = target as Spline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Vector3 point0 = ShowPoint(0);

        for (int i = 1; i < spline.ControlPointCount(); i += 3)
        {
            Vector3 point1 = ShowPoint(i);
            Vector3 point2 = ShowPoint(i + 1);
            Vector3 point3 = ShowPoint(i + 2);
          
            Handles.color = Color.gray;
            Handles.DrawLine(point0, point1);
            Handles.DrawLine(point2, point3);
          
            Handles.DrawBezier(point0, point3, point1, point2, Color.white, null, 2f);
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
        spline = target as Spline;

        EditorGUI.BeginChangeCheck();
        bool toggleDirections = EditorGUILayout.Toggle("Show Directions", showDirections);
        if (EditorGUI.EndChangeCheck())
        {
            showDirections = toggleDirections;
        }

        EditorGUI.BeginChangeCheck();
        bool realTime = EditorGUILayout.Toggle("Real Time Editing", realTimeEditing);
        if (EditorGUI.EndChangeCheck())
        {
            realTimeEditing = realTime;
        }

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }
       
        if(selectedIndex >= 0 && selectedIndex < spline.ControlPointCount())
        {
            DrawSelectedPointInspector();
        }
        if(GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }

        EditorGUI.BeginChangeCheck();
        float offsetFromFloor = spline.Offset();
        offsetFromFloor = EditorGUILayout.Slider("Offset", offsetFromFloor, 0.1f, 1000f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Offset");
            spline.SetOffset(offsetFromFloor);
            EditorUtility.SetDirty(spline);
        }

        GUILayout.Space(16f);

        GUILayout.Label("Reset Curve");
        if(GUILayout.Button("Reset"))
        {
            Undo.RecordObject(spline, "Reset");
            spline.Reset();
            EditorUtility.SetDirty(spline);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }

        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
        EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPointOnSpline(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);

        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = spline.GetPointOnSpline(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
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
         Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        
         float size = HandleUtility.GetHandleSize(point);
         //Increase the size of the first control point as looping will cause overlap.
         if (index == 0)
         {
             size *= 2f;
         }
         Handles.color = modeColours[(int)spline.GetControlPointMode(index)];
        
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
                 Undo.RecordObject(spline, "Move Point");
                 EditorUtility.SetDirty(spline);
        
                 //Transform to local space.
                 spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
             }
         }
        return point;
    }

}
