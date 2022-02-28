using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Line))]
public class NewBehaviourScript : Editor
{
    private void OnSceneGUI()
    {
        Line line = target as Line;

        Transform handleTransform = line.transform;
        //Respect whether we're in local or world space.
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        Vector3 wp0 = handleTransform.TransformPoint(line.point0);
        Vector3 wp1 = handleTransform.TransformPoint(line.point1);

        Handles.color = Color.white;
        Handles.DrawLine(wp0, wp1);
        //Handle's rotation from transform.
        Handles.DoPositionHandle(wp0, handleRotation);
        Handles.DoPositionHandle(wp1, handleRotation);

        //Update the canges within the Line class.
        EditorGUI.BeginChangeCheck();
        wp0 = Handles.DoPositionHandle(wp0, handleRotation);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            //Transform back into local space.
            line.point0 = handleTransform.InverseTransformPoint(wp0);
        }

        //Special editor methods to check if we have made a change to the line's transform.
        EditorGUI.BeginChangeCheck();
        wp0 = Handles.DoPositionHandle(wp1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            //Flag Unity to record the object's history so we can undo operations 'ctrl-z'.
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.point1 = handleTransform.InverseTransformPoint(wp1);
        }


    }
}
